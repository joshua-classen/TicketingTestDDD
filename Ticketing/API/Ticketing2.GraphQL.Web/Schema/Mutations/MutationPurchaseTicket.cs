using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Inputs;
using Ticketing2.GraphQL.Web.Repository;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationPurchaseTicket
{

    [Authorize(Roles = ["Kunde"])]
    public async Task<PurchasedTicket> BuyTicket(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        ClaimsPrincipal claimsPrincipal,
        
        BuyTicketCreateInput input)
    {
        var kunde = await KundeRepository.GetKunde(ticketingDbContext, userManager, claimsPrincipal);
        var veranstaltung = await VeranstaltungRepository.GetVeranstaltungById(ticketingDbContext, input.VeranstaltungId);
        await VeranstaltungRepository.EnsureVeranstaltungHasAtLeastOneMoreTicketToSell(ticketingDbContext, veranstaltung);
        
        var pTicket = new PurchasedTicket()
        {
            KundeUser = kunde,
            TicketNumber = veranstaltung.PurchasedTickets.Count + 1,
            PurchaseDate = DateTime.Now,
            TicketPriceEuroCent = 1999,
            Veranstaltung = veranstaltung
        };
        veranstaltung.PurchasedTickets.Add(pTicket); //BUG: Wird hier nicht gespeichert. // Problem liegt wahrscheinlich daran das PurchasedTicket keine Id hier bekommt.
        kunde.PurchasedTickets.Add(pTicket);
        await ticketingDbContext.SaveChangesAsync();

        return pTicket;
    }
}