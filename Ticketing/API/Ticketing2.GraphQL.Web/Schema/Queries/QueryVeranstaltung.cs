using HotChocolate.Authorization;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public IEnumerable<Veranstaltung> GetVeranstaltungen([Service] TicketingDbContext context)
    {
        return context.Veranstaltung.ToList();
    }
}