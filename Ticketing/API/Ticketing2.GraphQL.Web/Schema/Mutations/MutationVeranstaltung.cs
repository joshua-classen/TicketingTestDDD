using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.DomainObjects;
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
        
        VeranstaltungCreateInput input
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
        
        var veranstaltung = new Veranstaltung()
        {
            Name = input.Name
        };

        veranstalter.Veranstaltungen.Add(veranstaltung);
        await ticketingDbContext.SaveChangesAsync();

        return veranstaltung;
    }
}