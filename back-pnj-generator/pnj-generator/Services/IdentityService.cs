using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features.Identity;
using pnj_generator.Interfaces.Services;
using pnj_generator.Models.Features.Identities;

namespace pnj_generator.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly AppDbContext _db;

        public IdentityService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Identity> CreateFullIdentityAsync(Guid universeId, IdentityCreateDTO identityDto)
        {

            if (identityDto == null) return null;

            // 1. On récupère le genre global pour marquer nos fragments
            // (Tu pourrais aussi appeler ta méthode DetermineFragmentGender ici si tu l'as créée)
            Gender globalGender = identityDto.Gender;

            // 2. Assemblage de l'entité Identity
            // On appelle les méthodes privées (GetOrCreate...) qui sont dans la même classe
            var newIdentity = new Identity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Gender = globalGender,

                // Fragments (Nom, Prénom, Alias)
                FirstName = await GetOrCreateFragment(identityDto.FirstName, universeId, globalGender),
                Name = await GetOrCreateFragment(identityDto.Name, universeId, globalGender),
                Alias = await GetOrCreateFragment(identityDto.Alias, universeId, globalGender),

                // Informations additionnelles (Cultures, Espèces, etc.)
                // On utilise la méthode générique privée
                Culture = await GetOrCreateAdditionalInfo<Culture>(identityDto.Culture, universeId),
                Specie = await GetOrCreateAdditionalInfo<Specie>(identityDto.Specie, universeId),
                Alignment = await GetOrCreateAdditionalInfo<Alignment>(identityDto.Alignment, universeId),
                Origin = await GetOrCreateAdditionalInfo<Origin>(identityDto.Origin, universeId)
            };

            // 3. Enregistrement unique
            // EF Core est intelligent : il va détecter si les fragments/cultures 
            // sont nouveaux (Add) ou existants (Attach) et fera le nécessaire.
            _db.Identities.Add(newIdentity);
            await _db.SaveChangesAsync();

            return newIdentity;
        }

        public async Task<Identity?> UpdateIdentityAsync(Guid identityId, IdentityCreateDTO identityDto)
        {
            // 1. On récupère l'identité avec toutes ses dépendances
            var existingIdentity = await _db.Identities
                .Include(i => i.Name)
                .Include(i => i.FirstName)
                .Include(i => i.Alias)
                .Include(i => i.Culture)
                .Include(i => i.Specie)
                .Include(i => i.Alignment)
                .FirstOrDefaultAsync(i => i.Id == identityId);

            if (existingIdentity == null) return null;

            // 2. Mise à jour des données simples
            existingIdentity.Gender = identityDto.Gender;
            var universeId = existingIdentity.UniverseId;

            // 3. Mise à jour des fragments et infos (Réutilisation de tes méthodes privées)
            // C'est là que la magie opère : GetOrCreate va checker si le nouveau nom existe déjà
            existingIdentity.FirstName = await GetOrCreateFragment(identityDto.FirstName, universeId, identityDto.Gender);
            existingIdentity.Name = await GetOrCreateFragment(identityDto.Name, universeId, identityDto.Gender);
            existingIdentity.Alias = await GetOrCreateFragment(identityDto.Alias, universeId, identityDto.Gender);

            existingIdentity.Culture = await GetOrCreateAdditionalInfo<Culture>(identityDto.Culture, universeId);
            existingIdentity.Specie = await GetOrCreateAdditionalInfo<Specie>(identityDto.Specie, universeId);
            existingIdentity.Alignment = await GetOrCreateAdditionalInfo<Alignment>(identityDto.Alignment, universeId);
            existingIdentity.Origin = await GetOrCreateAdditionalInfo<Origin>(identityDto.Origin, universeId);

            // 4. Sauvegarde
            await _db.SaveChangesAsync();

            return existingIdentity;
        }

        private async Task<FragmentIdentity?> GetOrCreateFragment(FragmentIdentityDTO? dto, Guid universeId, Gender gender)
        {
            // Si l'utilisateur n'a rien saisi dans ce champ
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value)) return null;

            // Recherche si le fragment existe déjà (même valeur, même type, même univers)
            var existing = await _db.FragmentIdentities
                .FirstOrDefaultAsync(f => f.Value.ToLower() == dto.Value.ToLower()
                                       && f.UniverseId == universeId && f.Gender == gender);

            if (existing != null) return existing;

            // Sinon, création d'un nouveau fragment
            var newFragment = new FragmentIdentity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Value = dto.Value,
                Gender = gender
            };

            return newFragment;
        }

        private async Task<T?> GetOrCreateAdditionalInfo<T>(AdditionnalInformationDTO? dto, Guid universeId)
            where T : AdditionalInformation, new()
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value)) return null;

            // ✅ _db.Set<T>() au lieu de _db.AdditionalInformations
            // → Cherche dans la bonne table (cultures, species...), pas dans la base
            var existing = await _db.Set<T>()
                .FirstOrDefaultAsync(x => x.Value.ToLower() == dto.Value.ToLower()
                                       && x.UniverseId == universeId);

            if (existing != null) return existing;

            // ✅ new T() au lieu de new AdditionalInformation()
            // → Crée une Culture, Specie, etc. — pas le type de base
            var newInfo = new T
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Value = dto.Value
            };

            // ✅ _db.Set<T>() au lieu de _db.Set<AdditionalInformation>()
            // → Insère dans la bonne table dédiée
            return newInfo;
        }
    }
}
