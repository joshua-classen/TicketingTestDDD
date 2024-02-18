namespace Ticketing2.GraphQL.Web.DomainObjects;

public class Veranstaltung
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint TicketPriceEuroCent { get; set; }
    public uint MaxAmountTickets { get; set; }
    
    // Hier sollen die PurchasedTickets referenziert werden
    public ICollection<PurchasedTicket> PurchasedTickets { get; } = new List<PurchasedTicket>();
    
    
    // Überlegung: Hier die Tickets nicht vordefinieren sondern nach und nach hinzufügen.
    // oder vll doch alle vordefinieren??
}