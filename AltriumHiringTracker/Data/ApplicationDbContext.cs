using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AltriumHiringTracker.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Vacancy> Vacancies { get; set; } = default!;

        public DbSet<CandidateApplication> CandidateApplications { get; set; } = default!;
        public DbSet<Interview> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidateApplication>()
                .HasIndex(a => new { a.VacancyId, a.Email })
                .IsUnique();

            modelBuilder.Entity<CandidateApplication>()
                .Property(a => a.MatchScore)
                .HasPrecision(5, 2);
        }
    }
}