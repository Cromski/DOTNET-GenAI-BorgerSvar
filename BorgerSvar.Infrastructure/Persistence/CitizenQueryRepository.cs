using BorgerSvar.Application.Common.Interfaces;
using BorgerSvar.Domain;
using Microsoft.EntityFrameworkCore;

namespace BorgerSvar.Infrastructure.Persistence;

public class CitizenQueryRepository : ICitizenQueryRepository
{
    private readonly BorgerSvarDbContext _dbContext;

    public CitizenQueryRepository(BorgerSvarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default)
    {
        // Tilføjer den nye henvendelse til EF Core's sporing
        await _dbContext.CitizenQueries.AddAsync(citizenQuery, cancellationToken);
        
        // Gemmer ændringerne i SQLite databasen (skyder en SQL INSERT afsted)
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CitizenQuery?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Finder en henvendelse ud fra dens ID
        return await _dbContext.CitizenQueries
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default)
    {
        // Fortæller EF Core, at denne entitet er blevet ændret
        _dbContext.CitizenQueries.Update(citizenQuery);
        
        // Gemmer ændringerne (skyder en SQL UPDATE afsted)
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}