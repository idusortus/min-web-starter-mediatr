using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Quote> Quotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(k => k.Id);// Implicit, along with .ValueGeneratedOnAdd()
            entity.Property(c => c.Content).IsRequired();
            entity.Property(a => a.Author).IsRequired();                
        });
            
        base.OnModelCreating(modelBuilder);
    }
}