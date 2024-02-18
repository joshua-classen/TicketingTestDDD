using System.Data;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Repository;

public class VeranstaltungRepository
{
    
    // todo: verwende strongly typed Ids
    public static async Task<Veranstaltung> GetVeranstaltungById(
        TicketingDbContext ticketingDbContext, // kann der den hier auch injected bekommen?
        Guid id)
    {
        var veranstaltung = await ticketingDbContext.Veranstaltung.FindAsync(id);
        if (veranstaltung is null)
        {
            throw new Exception("Veranstaltung mit id: " + id + " nicht gefunden.");
        }

        return veranstaltung;
    }


    public static async Task EnsureVeranstaltungHasAtLeastOneMoreTicketToSell(
        TicketingDbContext ticketingDbContext,
        Veranstaltung veranstaltung)
    {
        // ich muss mir hier alle purchasedTickets der Veransatlung holen
        // Dann vergleiche ich die Anzahl der PurchasedTickets mit der maxAnzahl verkaufbarer Tickets in der Veranstaltung
        if(veranstaltung.PurchasedTickets.Count >= veranstaltung.MaxAmountTickets)
        {
            throw new Exception("Es sind keine Tickets mehr verfügbar.");
        }
        
    }
    
}