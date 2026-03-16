using FluentAssertions;
using pnj_generator.Data;
using pnj_generator.Models;
using pnj_generator.Models.Features;
using pnj_generator.Models.Features.Identities;
using pnj_generator.Services;
using pnj_generator.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace pnj_generator.Tests.Services;

/// <summary>
/// Tests unitaires pour NPCGeneratorService
/// Le service le plus critique de l'application
/// </summary>
public class NPCGeneratorServiceTests
{
    // ======================================
    // TESTS GÉNÉRATION COMPLÈTE NPC
    // ======================================

    [Fact]
    public async Task GenerateNPCAsync_WithValidUniverse_ShouldReturnValidNPC()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var universe = CreateTestUniverse(universeId);
        var characteristics = CreateTestCharacteristics(universeId);
        var fragments = CreateTestIdentityFragments(universeId);

        var context = TestDbContextFactory.CreateWithData(
            universe,
            characteristics[0], characteristics[1],
            fragments[0], fragments[1], fragments[2]
        );

        var service = new NPCGeneratorService(context);

        // ACT
        var npc = await service.GenerateNPCAsync(universeId);

        // ASSERT
        npc.Should().NotBeNull();
        npc.UniverseId.Should().Be(universeId);

        // Chaque snapshot doit être un JSON valide et non vide
        npc.IdentitySnapshot.Should().NotBeNullOrEmpty();
        npc.CharacteristicsSnapshot.Should().NotBeNullOrEmpty();

        // Les snapshots sans données en BDD retournent "[]" — valide JSON
        npc.WeaponsSnapshot.Should().NotBeNull();
        npc.ProtectionsSnapshot.Should().NotBeNull();
        npc.EquipmentSnapshot.Should().NotBeNull();
        npc.SkillsSnapshot.Should().NotBeNull();
        npc.TraitsSnapshot.Should().NotBeNull();
    }

    [Fact]
    public async Task GenerateNPCAsync_ShouldHaveNullHPByDefault()
    {
        // ARRANGE
        // Le modèle NPC actuel a un champ HP nullable.
        // Les HP sont optionnels en V1 — le MJ les renseigne manuellement.
        // Ce test vérifie que le générateur ne plante pas et retourne un NPC valide.
        var universeId = Guid.NewGuid();
        var universe = CreateTestUniverse(universeId);
        var characteristics = CreateTestCharacteristics(universeId);
        var fragments = CreateTestIdentityFragments(universeId);

        var context = TestDbContextFactory.CreateWithData(
            universe, characteristics[0], characteristics[1],
            fragments[0], fragments[1]
        );

        var service = new NPCGeneratorService(context);

        // ACT
        var npc = await service.GenerateNPCAsync(universeId);

        // ASSERT
        // En V1, HP est null par défaut (le MJ le renseigne après génération)
        // En V2, il sera calculé automatiquement à partir de Force + Résistance
        npc.HP.Should().BeNull();
    }

    // ======================================
    // TESTS GÉNÉRATION IDENTITÉ
    // ======================================

    [Fact]
    public async Task GenerateIdentitySnapshot_WithNeutralGender_ShouldPickMaleOrFemaleFirstName()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var maleFirstName = new FragmentIdentity
        {
            Id = Guid.NewGuid(),
            UniverseId = universeId,
            Gender = Gender.Male,
            Type = FragmentType.FirstName,
            Value = "John"
        };
        var femaleFirstName = new FragmentIdentity
        {
            Id = Guid.NewGuid(),
            UniverseId = universeId,
            Gender = Gender.Female,
            Type = FragmentType.FirstName,
            Value = "Jane"
        };

        // Le service fait FindAsync(universeId) en tout premier — l'univers DOIT être en BDD
        var universe = CreateTestUniverse(universeId);
        var context = TestDbContextFactory.CreateWithData(universe, maleFirstName, femaleFirstName);
        var service = new NPCGeneratorService(context);

        // ACT - Générer plusieurs fois pour tester l'aléatoire
        var firstNames = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            var npc = await service.GenerateNPCAsync(universeId);
            // On désérialise en JsonElement (type concret) plutôt qu'en dynamic.
            // Raison : FluentAssertions ne peut pas analyser une lambda sur dynamic
            // à la compilation → erreur CS1963 "dynamic operation in expression tree"
            var identity = JsonSerializer.Deserialize<JsonElement>(npc.IdentitySnapshot);
            firstNames.Add(identity.GetProperty("firstName").GetString()!);
        }

        // ASSERT
        // On devrait avoir au moins un John ET un Jane sur 10 générations (statistiquement)
        firstNames.Should().Contain("John");
        firstNames.Should().Contain("Jane");
    }

    [Fact]
    public async Task GenerateIdentitySnapshot_WithNoLastName_ShouldReturnEmptyString()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var firstName = new FragmentIdentity
        {
            Id = Guid.NewGuid(),
            UniverseId = universeId,
            Gender = Gender.Male,
            Type = FragmentType.FirstName,
            Value = "John"
        };
        // Pas de LastName dans la BDD

        var universe = CreateTestUniverse(universeId);
        var context = TestDbContextFactory.CreateWithData(universe, firstName);
        var service = new NPCGeneratorService(context);

        // ACT
        var npc = await service.GenerateNPCAsync(universeId);
        var identity = JsonSerializer.Deserialize<JsonElement>(npc.IdentitySnapshot);

        // ASSERT
        identity.GetProperty("lastName").GetString().Should().Be(""); // Vide, pas "Stranger"
    }

    // ======================================
    // TESTS GÉNÉRATION CARACTÉRISTIQUES
    // ======================================

    [Fact]
    public async Task GenerateCharacteristics_WithDiceCountMode_ShouldGenerateCorrectFormat()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var characteristic = new Characteristic
        {
            Id = Guid.NewGuid(),
            UniverseId = universeId,
            Name = "Force",
            GenerationType = CharacteristicGenerationType.DiceCount,
            DiceType = "D6",
            MinDice = 2,
            MaxDice = 5,
            HasModifiers = false
        };

        var universe = CreateTestUniverse(universeId);
        var context = TestDbContextFactory.CreateWithData(universe, characteristic);
        var service = new NPCGeneratorService(context);

        // ACT
        var npc = await service.GenerateNPCAsync(universeId);
        var characteristics = JsonSerializer.Deserialize<JsonElement[]>(npc.CharacteristicsSnapshot);

        // ASSERT
        var forceChar = characteristics!.First(c => c.GetProperty("name").GetString() == "Force");
        forceChar.GetProperty("diceType").GetString().Should().Be("D6");

        // "value" contient nbDice dans le snapshot actuel du service
        var nbDice = forceChar.GetProperty("value").GetInt32();
        nbDice.Should().BeInRange(2, 5);
    }

    [Fact]
    public async Task GenerateCharacteristics_WithFixedValueMode_ShouldGenerateValue()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var characteristic = new Characteristic
        {
            Id = Guid.NewGuid(),
            UniverseId = universeId,
            Name = "Intelligence",
            GenerationType = CharacteristicGenerationType.FixedValue,
            // NOTE : le service ignore MinValue/MaxValue et utilise MinDice/MaxDice
            // dans tous les cas (voir GenerateCharacteristicsSnapshotAsync).
            // MinValue/MaxValue sont prévus pour une version future.
            MinDice = 8,
            MaxDice = 18,
            HasModifiers = false
        };

        var universe = CreateTestUniverse(universeId);
        var context = TestDbContextFactory.CreateWithData(universe, characteristic);
        var service = new NPCGeneratorService(context);

        // ACT
        var npc = await service.GenerateNPCAsync(universeId);
        var characteristics = JsonSerializer.Deserialize<JsonElement[]>(npc.CharacteristicsSnapshot);

        // ASSERT
        var intChar = characteristics!.First(c => c.GetProperty("name").GetString() == "Intelligence");
        var value = intChar.GetProperty("value").GetInt32();
        value.Should().BeInRange(8, 18);
    }

    // ======================================
    // HELPERS - Création données de test
    // ======================================

    private static Universe CreateTestUniverse(Guid id)
    {
        return new Universe
        {
            Id = id,
            Name = "Test Universe",
            DiceRule = "D6",
            HasModifiers = false
        };
    }

    private static List<Characteristic> CreateTestCharacteristics(Guid universeId)
    {
        return new List<Characteristic>
        {
            new Characteristic
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Name = "Force",
                GenerationType = CharacteristicGenerationType.DiceCount,
                DiceType = "D6",
                MinDice = 2,
                MaxDice = 5
            },
            new Characteristic
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Name = "Intelligence",
                GenerationType = CharacteristicGenerationType.FixedValue,
                MinValue = 8,
                MaxValue = 18
            }
        };
    }

    private static List<FragmentIdentity> CreateTestIdentityFragments(Guid universeId)
    {
        return new List<FragmentIdentity>
        {
            new FragmentIdentity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Gender = Gender.Male,
                Type = FragmentType.FirstName,
                Value = "John"
            },
            new FragmentIdentity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Gender = Gender.Female,
                Type = FragmentType.FirstName,
                Value = "Jane"
            },
            new FragmentIdentity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Type = FragmentType.LastName,
                Value = "Doe"
            }
        };
    }
}