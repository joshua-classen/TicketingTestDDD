using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.Services;

public class TicketingDbContext : IdentityDbContext<IdentityUser>
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingDbContext).Assembly); // Was macht das hier? 
    }
    
    public DbSet<KundeUser> KundeUser { get; set; }
    public DbSet<VeranstalterUser> VeranstalterUser { get; set; }
    public DbSet<Veranstaltung> Veranstaltung { get; set; } // sollte man den Namen hier in Singular oder Plural schreiben?
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; } // ich glaube das muss hier rein
}