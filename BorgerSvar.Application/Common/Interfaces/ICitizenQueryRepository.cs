using BorgerSvar.Domain;

namespace BorgerSvar.Application.Common.Interfaces;

public interface ICitizenQueryRepository
{
    Task AddAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default);
    Task<CitizenQuery?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default);
}