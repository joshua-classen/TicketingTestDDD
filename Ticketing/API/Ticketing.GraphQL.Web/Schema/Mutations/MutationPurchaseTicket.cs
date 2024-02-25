using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Repository;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.Stripe;

namespace Ticketing.GraphQL.Web.Schema.Mutations;


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
        
        StripeService.StripePaymentIntent();
        // StripeService.StripeTest();
        // stripe testing
        
        
        var kunde = await KundeRepository.GetKunde(ticketingDbContext, userManager, claimsPrincipal);
        var veranstaltung = await VeranstaltungRepository.GetVeranstaltungById(ticketingDbContext, input.VeranstaltungId);
        await VeranstaltungRepository.EnsureVeranstaltungHasAtLeastOneMoreTicketToSell(ticketingDbContext, veranstaltung);
        
        var pTicket = new PurchasedTicket()
        {
            KundeUser = kunde,
            TicketNumber = Convert.ToUInt32(veranstaltung.PurchasedTickets.Count) + 1,
            PurchaseDate = DateTime.Now,
            TicketPriceEuroCent = veranstaltung.TicketPriceEuroCent,
            Veranstaltung = veranstaltung
        };
        veranstaltung.PurchasedTickets.Add(pTicket);
        kunde.PurchasedTickets.Add(pTicket);
        await ticketingDbContext.SaveChangesAsync();

        return pTicket;
    }
}