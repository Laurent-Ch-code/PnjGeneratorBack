using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.Interfaces.Services;
using pnj_generator.Models;
using pnj_generator.Models.Features;
using pnj_generator.Models.Features.Identities;
using pnj_generator.Models.Rules;
using System.Text.Json;

namespace pnj_generator.Services
{
    public class NPCGeneratorService : INPCGeneratorService
    {
        private readonly AppDbContext _db;
        private readonly Random _random = new Random();

        public NPCGeneratorService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Génère un PNJ complet aléatoirement pour un univers donné
        /// </summary>
        public async Task<NPC> GenerateNPCAsync(Guid universeId)
        {
            var universe = await _db.Universes.FindAsync(universeId);
            if (universe == null)
                throw new ArgumentException($"Universe {universeId} not found");

            // Génération de chaque composant
            var identitySnapshot = await GenerateIdentitySnapshotAsync(universeId);
            var characteristicsSnapshot = await GenerateCharacteristicsSnapshotAsync(universeId);
            var skillsSnapshot = await SelectRandomSkillsAsync(universeId);
            var weaponsSnapshot = await SelectRandomWeaponsAsync(universeId);
            var protectionsSnapshot = await SelectRandomProtectionsAsync(universeId);
            var equipmentSnapshot = await SelectRandomEquipmentAsync(universeId);
            var traitsSnapshot = await SelectRandomTraitsAsync(universeId);

            var npc = new NPC
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                IdentitySnapshot = identitySnapshot,
                CharacteristicsSnapshot = characteristicsSnapshot,
                SkillsSnapshot = skillsSnapshot,
                WeaponsSnapshot = weaponsSnapshot,
                ProtectionsSnapshot = protectionsSnapshot,
                EquipmentSnapshot = equipmentSnapshot,
                TraitsSnapshot = traitsSnapshot,
                CreatedAt = DateTime.UtcNow
            };

            return npc;
        }

        /// <summary>
        /// Génère une identité complète en piochant dans les fragments BDD
        /// </summary>
        private async Task<string> GenerateIdentitySnapshotAsync(Guid universeId)
        {
            // Tire un sexe aléatoire
            var gender = (Gender)_random.Next(0, 3);

            // Récupère des fragments selon le sexe ET le type
            var firstNames = await _db.FragmentIdentities
                .Where(f => f.UniverseId == universeId && f.Gender == gender && f.Type == FragmentType.FirstName)
                .ToListAsync();

            var lastNames = await _db.FragmentIdentities
                .Where(f => f.UniverseId == universeId && f.Type == FragmentType.LastName)
                .ToListAsync();

            var aliases = await _db.FragmentIdentities
                .Where(f => f.UniverseId == universeId && f.Type == FragmentType.Alias)
                .ToListAsync();

            var cultures = await _db.Set<Culture>()
                .Where(c => c.UniverseId == universeId)
                .ToListAsync();

            var species = await _db.Set<Specie>()
                .Where(s => s.UniverseId == universeId)
                .ToListAsync();

            var alignments = await _db.Set<Alignment>()
                .Where(a => a.UniverseId == universeId)
                .ToListAsync();

            var origins = await _db.Set<Origin>()
                .Where(o => o.UniverseId == universeId)
                .ToListAsync();

            // Sélection aléatoire
            var firstName = firstNames.Any() ? firstNames[_random.Next(firstNames.Count)].Value : "Unknown";
            var lastName = lastNames.Any() ? lastNames[_random.Next(lastNames.Count)].Value : "Stranger";
            var alias = _random.Next(0, 2) == 0 && aliases.Any() ? aliases[_random.Next(aliases.Count)].Value : null;
            var age = _random.Next(18, 60); // Adulte par défaut V1

            var identity = new
            {
                firstName,
                lastName,
                alias,
                age,
                gender = gender.ToString(),
                culture = cultures.Any() ? cultures[_random.Next(cultures.Count)].Value : null,
                specie = species.Any() ? species[_random.Next(species.Count)].Value : null,
                alignment = alignments.Any() ? alignments[_random.Next(alignments.Count)].Value : null,
                origin = origins.Any() ? origins[_random.Next(origins.Count)].Value : null
            };

            return JsonSerializer.Serialize(identity);
        }

        /// <summary>
        /// Génère les valeurs de TOUTES les caractéristiques de l'univers
        /// </summary>
        private async Task<string> GenerateCharacteristicsSnapshotAsync(Guid universeId)
        {
            var characteristics = await _db.Characteristics
                .Where(c => c.UniverseId == universeId)
                .ToListAsync();

            var results = new List<object>();

            foreach (var charact in characteristics)
            {
                // Jet de dés selon minDice/maxDice
                int nbDice = charact.MaxDice.HasValue && charact.MaxDice > charact.MinDice
                    ? _random.Next(charact.MinDice, charact.MaxDice.Value + 1)
                    : charact.MinDice;

                //int value = RollDice(nbDice, GetDiceSize(charact.DiceType));

                // Calcul du modificateur selon les règles
                int? modifier = await CalculateModifierAsync(universeId, charact.Id, nbDice);

                results.Add(new
                {
                    name = charact.Name,
                    diceType = charact.DiceType,
                    value = nbDice,
                    modifier
                });
            }

            return JsonSerializer.Serialize(results);
        }

        /// <summary>
        /// Sélectionne 2-3 compétences aléatoirement
        /// </summary>
        private async Task<string> SelectRandomSkillsAsync(Guid universeId)
        {
            var skills = await _db.Skills
                .Where(s => s.UniverseId == universeId)
                .ToListAsync();

            if (!skills.Any()) return "[]";

            int count = Math.Min(_random.Next(2, 4), skills.Count);
            var selected = skills.OrderBy(x => _random.Next()).Take(count);

            var result = selected.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                bonus = s.Bonus,
                malus = s.Malus
            });

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Sélectionne 1-2 armes aléatoirement
        /// </summary>
        private async Task<string> SelectRandomWeaponsAsync(Guid universeId)
        {
            var weapons = await _db.Weapons
                .Where(w => w.UniverseId == universeId)
                .ToListAsync();

            if (!weapons.Any()) return "[]";

            int count = Math.Min(_random.Next(1, 3), weapons.Count);
            var selected = weapons.OrderBy(x => _random.Next()).Take(count);

            var result = selected.Select(w => new
            {
                id = w.Id,
                name = w.Name,
                type = w.Type,
                damage = w.Damage,
                range = w.Range,
                capacity = w.Capacity,
                fireMode = w.WeaponFireMode.ToString()
            });

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Sélectionne 0-1 protection aléatoirement
        /// </summary>
        private async Task<string> SelectRandomProtectionsAsync(Guid universeId)
        {
            var protections = await _db.Protections
                .Where(p => p.UniverseId == universeId)
                .ToListAsync();

            if (!protections.Any() || _random.Next(0, 2) == 0) return "[]";

            var selected = protections[_random.Next(protections.Count)];

            var result = new[]
            {
                new
                {
                    id = selected.Id,
                    name = selected.Name,
                    type = selected.Type,
                    armorRating = selected.ArmorRating,
                    material = selected.Material,
                    weight = selected.Weight,
                    description = selected.Description
                }
            };

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Sélectionne 0-2 équipements aléatoirement
        /// </summary>
        private async Task<string> SelectRandomEquipmentAsync(Guid universeId)
        {
            var equipment = await _db.Equipments
                .Where(e => e.UniverseId == universeId)
                .ToListAsync();

            if (!equipment.Any()) return "[]";

            int count = Math.Min(_random.Next(0, 3), equipment.Count);
            if (count == 0) return "[]";

            var selected = equipment.OrderBy(x => _random.Next()).Take(count);

            var result = selected.Select(e => new
            {
                id = e.Id,
                name = e.Name,
                type = e.Type,
                bonusMalus = e.Bonus,
                malus = e.Malus
            });

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Sélectionne 0-2 traits aléatoirement
        /// </summary>
        private async Task<string> SelectRandomTraitsAsync(Guid universeId)
        {
            var traits = await _db.Traits
                .Where(t => t.UniverseId == universeId)
                .ToListAsync();

            if (!traits.Any()) return "[]";

            int count = Math.Min(_random.Next(0, 3), traits.Count);
            if (count == 0) return "[]";

            var selected = traits.OrderBy(x => _random.Next()).Take(count);

            var result = selected.Select(t => new
            {
                id = t.Id,
                name = t.Name,
                effect = t.Effect
            });

            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Calcule le modificateur selon les règles de l'univers ou de la caractéristique
        /// </summary>
        private async Task<int?> CalculateModifierAsync(Guid universeId, Guid characteristicId, int value)
        {
            // Règles spécifiques à la caractéristique (priorité)
            var rules = await _db.ModifierRules
                .Where(r => r.UniverseId == universeId && r.CharacteristicId == characteristicId)
                .ToListAsync();

            // Si pas de règles spécifiques, on prend les règles globales de l'univers
            if (!rules.Any())
            {
                rules = await _db.ModifierRules
                    .Where(r => r.UniverseId == universeId && r.CharacteristicId == null)
                    .ToListAsync();
            }

            if (!rules.Any()) return null;

            // Application selon le type
            var rule = rules.FirstOrDefault();
            if (rule == null) return null;

            if (rule.Type == ModifierType.RangeTable)
            {
                // Cherche dans quelle tranche se situe la valeur
                var match = rules.FirstOrDefault(r =>
                    r.RangeMin.HasValue && r.RangeMax.HasValue &&
                    value >= r.RangeMin.Value && value <= r.RangeMax.Value);

                return match?.Modifier;
            }
            else if (rule.Type == ModifierType.AvailableList)
            {
                // Sélectionne un modificateur disponible aléatoirement
                var availableValues = rules
                    .Where(r => r.AvailableValue.HasValue)
                    .Select(r => r.AvailableValue.Value)
                    .ToList();

                return availableValues.Any() ? availableValues[_random.Next(availableValues.Count)] : 0;
            }

            return null;
        }

        /// <summary>
        /// Lance N dés de taille S et retourne la somme
        /// </summary>
        private int RollDice(int count, int size)
        {
            int total = 0;
            for (int i = 0; i < count; i++)
                total += _random.Next(1, size + 1);
            return total;
        }

        /// <summary>
        /// Extrait la taille du dé depuis le string (ex: "D6" → 6)
        /// </summary>
        private int GetDiceSize(string diceType)
        {
            var cleaned = diceType.ToUpper().Replace("D", "").Trim();
            return int.TryParse(cleaned, out int size) ? size : 6;
        }
    }
}