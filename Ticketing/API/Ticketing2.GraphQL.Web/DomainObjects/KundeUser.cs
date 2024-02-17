namespace Ticketing2.GraphQL.Web.DomainObjects;

public class KundeUser
{
    public int Id { get; set; }
    
    // als Fremdschlüssel mappen
    public string AspNetUserId { get; set; }
    
    
    // Der Kunde kann nur Tickets haben wenn er sie gekauft hat.

    
    // Hier sollen die PurchasedTickets referenziert werden
    public ICollection<PurchasedTicket> PurchasedTickets { get; } = new List<PurchasedTicket>();

    // Wenn ich ein purchasedTicket habe, dann brauche ich eig kein Ticket in der Veranstaltung mehr. Das PurchasedTicket referenziert die Veranstaltung. 

}