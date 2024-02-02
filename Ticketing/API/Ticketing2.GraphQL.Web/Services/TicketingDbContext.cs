using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.DTOs;

namespace Ticketing2.GraphQL.Web.Services;

public class TicketingDbContext : DbContext
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
    {
    }
    
    public DbSet<VeranstalterDTO> Veranstalter { get; set; }
    
}