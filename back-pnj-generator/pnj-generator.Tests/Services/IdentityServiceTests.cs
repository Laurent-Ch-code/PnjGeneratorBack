using FluentAssertions;
using pnj_generator.DTOs.Features.Identity;
using pnj_generator.Models;
using pnj_generator.Models.Features.Identities;
using pnj_generator.Services;
using pnj_generator.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace pnj_generator.Tests.Services;

/// <summary>
/// Tests unitaires pour IdentityService.
/// Méthodes testées : CreateFullIdentityAsync, UpdateIdentityAsync
///
/// NOTE sur GetOrCreateFragment :
/// Le service crée les fragments en mémoire mais NE les persiste PAS
/// indépendamment — ils sont attachés à l'Identity et sauvegardés avec elle.
/// On teste donc le comportement observable (l'Identity est bien créée),
/// pas l'implémentation interne.
/// </summary>
public class IdentityServiceTests
{
    // ─── Helper : univers minimal requis par la FK de Identity ───────────────
    private static Universe MakeUniverse(Guid? id = null) => new Universe
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Universe",
        DiceRule = "D6"
    };

    // ─── Helper : DTO minimal valide ─────────────────────────────────────────
    private static IdentityCreateDTO MakeDTO(
        string firstName = "John",
        string lastName = "Doe",
        Gender gender = Gender.Male,
        int? age = 30,
        string? description = null) => new IdentityCreateDTO
        {
            Gender = gender,
            Age = age,
            Description = description,
            FirstName = new FragmentIdentityDTO { Value = firstName },
            Name = new FragmentIdentityDTO { Value = lastName },
        };

    // ======================================
    // TESTS CreateFullIdentityAsync
    // ======================================

    [Fact]
    public async Task CreateFullIdentityAsync_WithValidDTO_ShouldReturnIdentityWithId()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        // ACT
        var result = await service.CreateFullIdentityAsync(universeId, MakeDTO("Marcus", "Kane", Gender.Male, 35));

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.UniverseId.Should().Be(universeId);
        result.Gender.Should().Be(Gender.Male);
        result.Age.Should().Be(35);
    }

    [Fact]
    public async Task CreateFullIdentityAsync_ShouldPersistIdentityInDatabase()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        // ACT
        var result = await service.CreateFullIdentityAsync(universeId, MakeDTO());

        // ASSERT — on relit depuis le DbContext pour confirmer la persistance
        var saved = await context.Identities.FindAsync(result.Id);
        saved.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateFullIdentityAsync_WithNullDTO_ShouldReturnNull()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        // ACT
        // Le service a un guard clause explicite : if (identityDto == null) return null
        var result = await service.CreateFullIdentityAsync(universeId, null!);

        // ASSERT
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateFullIdentityAsync_WithOptionalFieldsNull_ShouldNotSetThem()
    {
        // ARRANGE — DTO sans alias, culture, espèce, alignement, origine
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        var dto = new IdentityCreateDTO
        {
            Gender = Gender.Female,
            FirstName = new FragmentIdentityDTO { Value = "Sarah" },
            // Tout le reste null intentionnellement
        };

        // ACT
        var result = await service.CreateFullIdentityAsync(universeId, dto);

        // ASSERT — les champs optionnels restent null
        result.Should().NotBeNull();
        result.Alias.Should().BeNull();
        result.Culture.Should().BeNull();
        result.Specie.Should().BeNull();
        result.Alignment.Should().BeNull();
        result.Origin.Should().BeNull();
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Neutral)]
    public async Task CreateFullIdentityAsync_ShouldSaveCorrectGender(Gender gender)
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        var dto = MakeDTO(gender: gender);

        // ACT
        var result = await service.CreateFullIdentityAsync(universeId, dto);

        // ASSERT
        result.Gender.Should().Be(gender);
    }

    [Fact]
    public async Task CreateFullIdentityAsync_MultipleTimes_ShouldCreateDistinctIdentities()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var context = TestDbContextFactory.CreateWithData(MakeUniverse(universeId));
        var service = new IdentityService(context);

        // ACT — deux identités avec des noms différents
        var identity1 = await service.CreateFullIdentityAsync(universeId, MakeDTO("John", "Doe"));
        var identity2 = await service.CreateFullIdentityAsync(universeId, MakeDTO("Jane", "Smith", Gender.Female));

        // ASSERT — chaque appel produit une Identity distincte
        identity1.Id.Should().NotBe(identity2.Id);
        context.Identities.Should().HaveCount(2);
    }

    // ======================================
    // TESTS UpdateIdentityAsync
    // ======================================

    [Fact]
    public async Task UpdateIdentityAsync_WithUnknownId_ShouldReturnNull()
    {
        // ARRANGE — BDD vide, l'Id passé n'existe pas
        var context = TestDbContextFactory.CreateInMemoryContext();
        var service = new IdentityService(context);

        // ACT
        var result = await service.UpdateIdentityAsync(Guid.NewGuid(), MakeDTO());

        // ASSERT — le service retourne null si introuvable
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateIdentityAsync_WithExistingId_ShouldUpdateGenderAndAge()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var universe = MakeUniverse(universeId);

        // On crée l'Identity via le service plutôt qu'en l'injectant brute —
        // cela garantit que EF Core la track correctement avec toutes ses FK.
        var context = TestDbContextFactory.CreateWithData(universe);
        var service = new IdentityService(context);

        // Création initiale
        var created = await service.CreateFullIdentityAsync(universeId, new IdentityCreateDTO
        {
            Gender = Gender.Male,
            Age = 25,
        });

        var updateDto = new IdentityCreateDTO
        {
            Gender = Gender.Female,
            Age = 40,
        };

        // ACT
        var result = await service.UpdateIdentityAsync(created.Id, updateDto);

        // ASSERT
        result.Should().NotBeNull();
        result!.Gender.Should().Be(Gender.Female);
        result.Age.Should().Be(40);
    }

    [Fact]
    public async Task UpdateIdentityAsync_ShouldPersistDescriptionChange()
    {
        // ARRANGE
        var universeId = Guid.NewGuid();
        var universe = MakeUniverse(universeId);

        var context = TestDbContextFactory.CreateWithData(universe);
        var service = new IdentityService(context);

        var created = await service.CreateFullIdentityAsync(universeId, new IdentityCreateDTO
        {
            Gender = Gender.Male,
            Description = "Description originale"
        });

        // ACT
        await service.UpdateIdentityAsync(created.Id, new IdentityCreateDTO
        {
            Gender = Gender.Male,
            Description = "Description modifiée"
        });

        // ASSERT — on relit depuis la BDD pour confirmer la persistance
        var saved = await context.Identities.FindAsync(created.Id);
        saved!.Description.Should().Be("Description modifiée");
    }
}