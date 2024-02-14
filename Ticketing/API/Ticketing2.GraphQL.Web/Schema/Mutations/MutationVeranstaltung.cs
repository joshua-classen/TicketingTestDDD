using HotChocolate.Authorization;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<Veranstaltung> CreateVeranstaltung(
        [Service] TicketingDbContext context,
        
        VeranstaltungCreateInput input
    )
    {
        var veranstaltung = new Veranstaltung()
        {
            Name = input.Name
        };
        context.Veranstaltung.Add(veranstaltung);
        await context.SaveChangesAsync();

        return veranstaltung;
    }
}