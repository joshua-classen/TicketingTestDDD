using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryTicket
{
    public IEnumerable<Ticket> GetTickets([Service] TicketingDbContext context)
    {
        return context.Ticket.ToList();
    }
}