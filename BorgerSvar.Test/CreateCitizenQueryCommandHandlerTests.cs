using BorgerSvar.Application.CitizenQueries.Commands;
using BorgerSvar.Application.Common.Interfaces;
using BorgerSvar.Domain;
using NSubstitute;
using Xunit;

namespace BorgerSvar.Test;

public class CreateCitizenQueryCommandHandlerTests
{
    private readonly ICitizenQueryRepository _repositoryMock;
    private readonly IAIService _aiServiceMock;
    private readonly CreateCitizenQueryCommandHandler _handler;

    public CreateCitizenQueryCommandHandlerTests()
    {
        // 1. Vi opretter vores "falske" afhængigheder med NSubstitute
        _repositoryMock = Substitute.For<ICitizenQueryRepository>();
        _aiServiceMock = Substitute.For<IAIService>();

        // 2. Vi instansierer vores handler og sniger vores mocks ind i den
        _handler = new CreateCitizenQueryCommandHandler(_repositoryMock, _aiServiceMock);
    }

    [Fact]
    public void Handle_WithValidCommand_ShouldCreateSaveAndReturnId()
    {
        // Arrange
        var command = new CreateCitizenQueryCommand(
            FirstName: "Jens",
            LastName: "Hansen",
            Email: "jens@example.com",
            Question: "Hvornår tømmes min skraldespand?"
        );

        var expectedAnswer = "Din skraldespand bliver tømt på torsdag.";

        // Vi fortæller vores AI-mock præcis, hvad den skal svare, når den bliver kaldt
        _aiServiceMock
            .GenerateResponseAsync(Arg.Any<CitizenQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedAnswer));

        // Act
        var resultId = _handler.Handle(command, CancellationToken.None).Result;

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);

        // Vi verificerer, at vores repository rent faktisk blev bedt om at gemme (AddAsync) én gang
        _repositoryMock
            .Received(1)
            .AddAsync(Arg.Any<CitizenQuery>(), Arg.Any<CancellationToken>());

        // Vi verificerer, at repository også fik besked på at opdatere med AI-svaret bagefter
        _repositoryMock
            .Received(1)
            .UpdateAsync(Arg.Any<CitizenQuery>(), Arg.Any<CancellationToken>());
    }
}