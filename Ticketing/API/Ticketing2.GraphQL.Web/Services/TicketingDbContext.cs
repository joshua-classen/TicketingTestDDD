using Microsoft.EntityFrameworkCore;

namespace Ticketing2.GraphQL.Web.Services;

public class TicketingDbContext : DbContext
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingDbContext).Assembly);
    }
    
    public DbSet<DomainObjects.Veranstalter> Veranstalter { get; set; }
}