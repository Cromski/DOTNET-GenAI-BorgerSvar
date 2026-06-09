namespace BorgerSvar.Domain;

public record CitizenIdCard
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }

    private CitizenIdCard(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static CitizenIdCard Create(string firstName, string lastName, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        if (!email.Contains("@"))
        {
            throw new ArgumentException("Email must contain '@' character.", nameof(email));
        }

        return new CitizenIdCard(firstName, lastName, email);
    }
}
