using HotChocolate.Authorization;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.Repository;

namespace Ticketing.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryKundeUser
{
    [Authorize(Roles = ["Kunde"])]
    public async Task<KundeUser> GetKundeUser(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var kunde = await KundeRepository.GetKunde(ticketingDbContext, userManager, claimsPrincipal);
        return kunde;
    }
}