using JobManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.DataAccess;

public class JobManagementContext(DbContextOptions<JobManagementContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxInformation>()
            .HasOne(t => t.Job).WithMany(j => j.TaxInformation)
            .HasForeignKey(ti => ti.JobId);

        modelBuilder.Entity<TaxRegime>()
            .HasMany(tr => tr.TaxInformations)
            .WithOne(ti => ti.TaxRegime)
            .HasForeignKey(ti => ti.TaxRegimeId);
    }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<TaxRegime> TaxRegimes { get; set; }
    internal DbSet<JobRole> JobRoles { get; set; }
    internal DbSet<TaxInformation> TaxInformations { get; set; }
}
