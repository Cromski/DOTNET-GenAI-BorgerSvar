using BorgerSvar.Domain;
using Microsoft.EntityFrameworkCore;

namespace BorgerSvar.Infrastructure.Persistence;

public class BorgerSvarDbContext : DbContext
{
    public BorgerSvarDbContext(DbContextOptions<BorgerSvarDbContext> options)
        : base(options)
    {
    }

    // Her fortæller vi EF Core, at vi vil have en tabel til vores CitizenQueries
    public DbSet<CitizenQuery> CitizenQueries => Set<CitizenQuery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Her konfigurerer vi, hvordan vores domænemodel skal gemmes i databasen
        modelBuilder.Entity<CitizenQuery>(entity =>
        {
            entity.HasKey(q => q.Id);

            // Da CitizenIdCard er et Value Object, kan vi "eje" det direkte i tabellen (Owned Entity)
            // Det betyder, at fornavn, efternavn og email gemmes som kolonner direkte på CitizenQuery-rækken
            entity.OwnsOne(q => q.CitizenIdCard, cb =>
            {
                cb.Property(c => c.FirstName).HasColumnName("Citizen_FirstName");
                cb.Property(c => c.LastName).HasColumnName("Citizen_LastName");
                cb.Property(c => c.Email).HasColumnName("Citizen_Email");
            });

            entity.Property(q => q.Question).IsRequired();
            entity.Property(q => q.Answer).IsRequired(false); // Kan være null før AI har svaret
            
            // Vi gemmer vores Enum (Status) som en tekststreng i databasen i stedet for et tal (mere læsbart)
            entity.Property(q => q.Status)
                  .HasConversion<string>();
        });
    }
}