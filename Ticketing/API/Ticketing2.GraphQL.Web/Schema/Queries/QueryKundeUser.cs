using HotChocolate.Authorization;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.Repository;


namespace Ticketing2.GraphQL.Web.Schema.Queries;

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