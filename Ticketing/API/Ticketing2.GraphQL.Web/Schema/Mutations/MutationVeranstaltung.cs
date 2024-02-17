using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Inputs;
using Ticketing2.GraphQL.Web.Repository;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;

[ExtendObjectType(Name = "Mutation")]
public class MutationVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<Veranstaltung> CreateVeranstaltung(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        ClaimsPrincipal claimsPrincipal,
        
        VeranstaltungCreateInput input)
    {
        var veranstalter = await VeranstalterRepository.GetVeranstalter(ticketingDbContext, userManager, claimsPrincipal);
        
        var veranstaltung = new Veranstaltung()
        {
            Name = input.Name
        };
        veranstalter.Veranstaltungen.Add(veranstaltung);
        await ticketingDbContext.SaveChangesAsync();
        
        return veranstaltung;
    }
}