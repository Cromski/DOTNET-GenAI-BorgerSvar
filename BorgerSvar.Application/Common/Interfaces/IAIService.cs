using BorgerSvar.Domain;

namespace BorgerSvar.Application.Common.Interfaces;

public interface IAIService
{
    Task<string> GenerateResponseAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default);
}