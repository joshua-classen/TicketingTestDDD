using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Services;

namespace Ticketing.GraphQL.Web.Repository;

public static class VeranstalterRepository
{
    public static async Task<VeranstalterUser> GetVeranstalter(
        TicketingDbContext ticketingDbContext,
        UserManager<IdentityUser> userManager,
        
        ClaimsPrincipal claimsPrincipal)
    {
        var aspNetUser = await userManager.GetUserAsync(claimsPrincipal);
        if (aspNetUser is null)
        {
            throw new Exception("AspNetUser nicht gefunden.");
        }
        
        var veranstalter = ticketingDbContext.VeranstalterUser
            .Include(vu => vu.Veranstaltungen)
            .ThenInclude(v => v.PurchasedTickets)
            .FirstOrDefault(vu => vu.AspNetUserId == aspNetUser.Id);
        if (veranstalter is null)
        {
            throw new Exception("Veranstalter nicht gefunden.");
        }

        return veranstalter;
    }
}