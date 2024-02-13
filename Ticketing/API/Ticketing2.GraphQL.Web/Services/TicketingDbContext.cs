using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.authenticationKram;

namespace Ticketing2.GraphQL.Web.Services;


// Durch IdentityDbContext<ApplicationUser> wird die Identity-Struktur in die Datenbank eingebunden
public class TicketingDbContext : IdentityDbContext<IdentityUser>
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