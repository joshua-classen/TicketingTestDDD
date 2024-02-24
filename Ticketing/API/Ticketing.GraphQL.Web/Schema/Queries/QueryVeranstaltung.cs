using HotChocolate.Authorization;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.Repository;

namespace Ticketing.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<IEnumerable<Veranstaltung>> GetVeranstaltungen(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var veranstalter = await VeranstalterRepository.GetVeranstalter(ticketingDbContext, userManager, claimsPrincipal);

        return veranstalter.Veranstaltungen.ToList();
    }
}