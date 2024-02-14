using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;

[ExtendObjectType(Name = "Mutation")]
public class MutationTicket
{
    
    // erst später mit Authorize
    public async Task<Ticket> CreateTicket(
        [Service] TicketingDbContext context,
        
        TicketCreateInput input
    )
    {
        var ticket = new Ticket()
        {
            Name = input.Name
        };
        context.Ticket.Add(ticket);
        await context.SaveChangesAsync();

        return ticket;
    }
}