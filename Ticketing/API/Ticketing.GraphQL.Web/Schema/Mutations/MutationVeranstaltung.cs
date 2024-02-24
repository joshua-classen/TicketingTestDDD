using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Repository;
using Ticketing.GraphQL.Web.Services;

namespace Ticketing.GraphQL.Web.Schema.Mutations;

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
            Name = input.Name,
            TicketPriceEuroCent = input.TicketPriceEuroCent,
            MaxAmountTickets = input.MaxAmountTickets
        };
        
        veranstalter.Veranstaltungen.Add(veranstaltung);
        await ticketingDbContext.SaveChangesAsync();
        
        return veranstaltung;
        // todo: sollte ich hier vll alle Veranstaltungen von dem Veranstalter zurückgeben?
    }
}