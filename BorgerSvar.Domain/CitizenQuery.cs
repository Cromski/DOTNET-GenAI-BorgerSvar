using System.Diagnostics.CodeAnalysis;

namespace BorgerSvar.Domain;

public class CitizenQuery
{
    public Guid Id { get; private set; }
    public CitizenIdCard CitizenIdCard { get; private set; }
    public string Question { get; private set; }
    public string? Answer { get; private set; }
    public QueryStatus Status { get; private set; }

    [SetsRequiredMembers]
    private CitizenQuery(CitizenIdCard citizenIdCard, string question)
    {
        Id = Guid.NewGuid();
        CitizenIdCard = citizenIdCard;
        Question = question;
        Answer = null;
        Status = QueryStatus.Behandles;
    }

    public static CitizenQuery Create(CitizenIdCard citizenIdCard, string question)
    {
        ArgumentNullException.ThrowIfNull(citizenIdCard);
        ArgumentException.ThrowIfNullOrWhiteSpace(question);

        return new CitizenQuery(citizenIdCard, question);
    }
}
