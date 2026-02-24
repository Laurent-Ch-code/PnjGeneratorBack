using Microsoft.AspNetCore.Mvc.RazorPages;
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

            Gender globalGender = identityDto.Gender;

            var newIdentity = new Identity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Gender = globalGender,

                // Fragments avec types spécifiques
                FirstName = await GetOrCreateFragment(identityDto.FirstName, universeId, globalGender, FragmentType.FirstName),
                Name = await GetOrCreateFragment(identityDto.Name, universeId, globalGender, FragmentType.LastName),
                Alias = await GetOrCreateFragment(identityDto.Alias, universeId, globalGender, FragmentType.Alias),

                Culture = await GetOrCreateAdditionalInfo<Culture>(identityDto.Culture, universeId),
                Specie = await GetOrCreateAdditionalInfo<Specie>(identityDto.Specie, universeId),
                Alignment = await GetOrCreateAdditionalInfo<Alignment>(identityDto.Alignment, universeId),
                Origin = await GetOrCreateAdditionalInfo<Origin>(identityDto.Origin, universeId),
                Age = identityDto.Age,
                Description = identityDto.Description
            };

            _db.Identities.Add(newIdentity);
            await _db.SaveChangesAsync();

            return newIdentity;
        }

        public async Task<Identity?> UpdateIdentityAsync(Guid identityId, IdentityCreateDTO identityDto)
        {
            var existingIdentity = await _db.Identities
                .Include(i => i.Name)
                .Include(i => i.FirstName)
                .Include(i => i.Alias)
                .Include(i => i.Culture)
                .Include(i => i.Specie)
                .Include(i => i.Alignment)
                .FirstOrDefaultAsync(i => i.Id == identityId);

            if (existingIdentity == null) return null;

            existingIdentity.Gender = identityDto.Gender;
            var universeId = existingIdentity.UniverseId;

            // Mise à jour avec types spécifiques
            existingIdentity.FirstName = await GetOrCreateFragment(identityDto.FirstName, universeId, identityDto.Gender, FragmentType.FirstName);
            existingIdentity.Name = await GetOrCreateFragment(identityDto.Name, universeId, identityDto.Gender, FragmentType.LastName);
            existingIdentity.Alias = await GetOrCreateFragment(identityDto.Alias, universeId, identityDto.Gender, FragmentType.Alias);

            existingIdentity.Culture = await GetOrCreateAdditionalInfo<Culture>(identityDto.Culture, universeId);
            existingIdentity.Specie = await GetOrCreateAdditionalInfo<Specie>(identityDto.Specie, universeId);
            existingIdentity.Alignment = await GetOrCreateAdditionalInfo<Alignment>(identityDto.Alignment, universeId);
            existingIdentity.Origin = await GetOrCreateAdditionalInfo<Origin>(identityDto.Origin, universeId);
            existingIdentity.Age = identityDto.Age;
            existingIdentity.Description = identityDto.Description;

            await _db.SaveChangesAsync();
            return existingIdentity;
        }

        /// <summary>
        /// Récupère ou crée un fragment d'identité avec type
        /// </summary>
        private async Task<FragmentIdentity?> GetOrCreateFragment(
            FragmentIdentityDTO? dto,  // ← corriger ici
            Guid universeId,
            Gender gender,
            FragmentType type)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value)) return null;

            // Recherche avec type spécifique
            var existing = await _db.FragmentIdentities
                .FirstOrDefaultAsync(f =>
                    f.Value.ToLower() == dto.Value.ToLower() &&
                    f.UniverseId == universeId &&
                    f.Gender == gender &&
                    f.Type == type);

            if (existing != null) return existing;

            // Création avec type
            var newFragment = new FragmentIdentity
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Value = dto.Value,
                Gender = gender,
                Type = type
            };

            return newFragment;
        }

        private async Task<T?> GetOrCreateAdditionalInfo<T>(AdditionnalInformationDTO? dto, Guid universeId)
            where T : AdditionalInformation, new()
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Value)) return null;

            var existing = await _db.Set<T>()
                .FirstOrDefaultAsync(x => x.Value.ToLower() == dto.Value.ToLower()
                                       && x.UniverseId == universeId);

            if (existing != null) return existing;

            var newInfo = new T
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Value = dto.Value
            };

            return newInfo;
        }
    }
}