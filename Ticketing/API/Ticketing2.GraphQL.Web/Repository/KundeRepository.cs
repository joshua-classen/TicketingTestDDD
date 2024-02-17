using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;
using Microsoft.EntityFrameworkCore;


namespace Ticketing2.GraphQL.Web.Repository;

public static class KundeRepository
{
    public static async Task<KundeUser> GetKunde(
        TicketingDbContext ticketingDbContext, // hier später ITicketingDBContext machen für DP Injection
        UserManager<IdentityUser> userManager,

        ClaimsPrincipal claimsPrincipal)
    {
        var aspNetUser = await userManager.GetUserAsync(claimsPrincipal);
        if (aspNetUser is null)
        {
            throw new Exception("AspNetUser nicht gefunden.");
        }
        // kann ich hier schon gucken ob der User die Rollen hat? ist das sinnvol das hier zu prüfen?
        
        var kunde = ticketingDbContext.KundeUser
            .Include(ku => ku.PurchasedTickets)
            .FirstOrDefault(ku => ku.AspNetUserId == aspNetUser.Id);
        if (kunde is null)
        {
            throw new Exception("Kunde nicht gefunden.");
        }

        return kunde;
    }
}