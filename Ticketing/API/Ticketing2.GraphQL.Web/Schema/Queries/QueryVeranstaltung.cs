using HotChocolate.Authorization;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Ticketing2.GraphQL.Web.Schema.Queries;

[ExtendObjectType(Name = "Query")]
public class QueryVeranstaltung
{
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<IEnumerable<Veranstaltung>> GetVeranstaltungen(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        ClaimsPrincipal claimsPrincipal
        )
    {
        var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        
        if (emailClaim is null)
        {
            throw new Exception("Email nicht im jwt token gefunden.");
        }
        
        var emailString = emailClaim.Value;
        var aspNetUser = await userManager.FindByEmailAsync(emailString);
        if (aspNetUser is null)
        {
            throw new Exception("User nicht gefunden.");
        }
        await ticketingDbContext.VeranstalterUser.ToListAsync();
        
        var veranstalter = ticketingDbContext.VeranstalterUser
            .Include(vu => vu.Veranstaltungen)
            .FirstOrDefault(vu => vu.AspNetUserId == aspNetUser.Id);
        
        if (veranstalter is null)
        {
            throw new Exception("Veranstalter nicht gefunden.");
        }

        return veranstalter.Veranstaltungen.ToList();
    }
}