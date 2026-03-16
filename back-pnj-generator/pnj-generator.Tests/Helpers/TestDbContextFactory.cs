using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using System;

namespace pnj_generator.Tests.Helpers;

/// <summary>
/// Factory pour créer des DbContext en mémoire pour les tests
/// </summary>
public static class TestDbContextFactory
{
    /// <summary>
    /// Crée un DbContext vide en mémoire avec un nom unique
    /// </summary>
    public static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Nom unique pour isoler les tests
            .Options;

        return new AppDbContext(options);
    }

    /// <summary>
    /// Crée un DbContext pré-rempli avec des entités
    /// </summary>
    public static AppDbContext CreateWithData(params object[] entities)
    {
        var context = CreateInMemoryContext();
        context.AddRange(entities);
        context.SaveChanges();
        return context;
    }
}