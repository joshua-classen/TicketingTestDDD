using HotChocolate.Authorization;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.Repository;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<IEnumerable<Veranstaltung>> GetVeranstaltungen(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<ApplicationUser> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var veranstalter = await VeranstalterRepository.GetVeranstalter(ticketingDbContext, userManager, claimsPrincipal);

        return veranstalter.Veranstaltungen.ToList();
    }
}