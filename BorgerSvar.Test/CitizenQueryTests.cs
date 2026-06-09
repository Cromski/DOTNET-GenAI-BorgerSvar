using BorgerSvar.Domain;

namespace BorgerSvar.Test;

public class CitizenQueryTests
{
    private static CitizenIdCard ValidCard() =>
        CitizenIdCard.Create("Hans", "Jensen", "hans@example.com");

    [Fact]
    public void Create_WithValidInput_ReturnsQuery()
    {
        var card = ValidCard();

        var query = CitizenQuery.Create(card, "Hvornår bliver mit affald hentet?");

        Assert.NotEqual(Guid.Empty, query.Id);
        Assert.Equal(card, query.CitizenIdCard);
        Assert.Equal("Hvornår bliver mit affald hentet?", query.Question);
        Assert.Null(query.Answer);
        Assert.Equal(QueryStatus.Behandles, query.Status);
    }

    [Fact]
    public void Create_WithNullCitizenIdCard_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            CitizenQuery.Create(null!, "Et spørgsmål"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidQuestion_Throws(string? question)
    {
        var card = ValidCard();

        Assert.Throws<ArgumentException>(() =>
            CitizenQuery.Create(card, question!));
    }

    [Fact]
    public void Create_WithNullQuestion_Throws()
    {
        var card = ValidCard();

        Assert.Throws<ArgumentNullException>(() =>
            CitizenQuery.Create(card, null!));
    }

    [Fact]
    public void Create_TwoQueries_HaveUniqueIds()
    {
        var card = ValidCard();

        var q1 = CitizenQuery.Create(card, "Spørgsmål 1");
        var q2 = CitizenQuery.Create(card, "Spørgsmål 2");

        Assert.NotEqual(q1.Id, q2.Id);
    }
}