using BorgerSvar.Application.Common.Interfaces;
using BorgerSvar.Domain;
using MediatR;

namespace BorgerSvar.Application.CitizenQueries.Commands;

// 1. Vores Command (De data vi modtager udefra)
// Vi siger til MediatR, at når denne kommando køres, returnerer den et Guid (ID'et på den oprettede henvendelse)
public record CreateCitizenQueryCommand(
    string FirstName,
    string LastName,
    string Email,
    string Question
) : IRequest<Guid>;

// 2. Vores Handler (Logikken der udfører handlingen)
public class CreateCitizenQueryCommandHandler : IRequestHandler<CreateCitizenQueryCommand, Guid>
{
    private readonly ICitizenQueryRepository _repository;
    private readonly IAIService _aiService;

    public CreateCitizenQueryCommandHandler(ICitizenQueryRepository repository, IAIService aiService)
    {
        _repository = repository;
        _aiService = aiService;
    }

    public async Task<Guid> Handle(CreateCitizenQueryCommand request, CancellationToken cancellationToken)
    {
        // Valider input
        var idCard = CitizenIdCard.Create(request.FirstName, request.LastName, request.Email);
        var citizenQuery = CitizenQuery.Create(idCard, request.Question);

        // Gem i repository
        await _repository.AddAsync(citizenQuery, cancellationToken);

        // Generer svar ved hjælp af AI 
        var answer = await _aiService.GenerateResponseAsync(citizenQuery, cancellationToken);

        // Opdater henvendelsen med svaret
        citizenQuery.AssignAIAnswer(answer);

        // Opdater i repository
        await _repository.UpdateAsync(citizenQuery, cancellationToken);

        // Returner ID'et på den oprettede entitet
        return citizenQuery.Id;
    }
}