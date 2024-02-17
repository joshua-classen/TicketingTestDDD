namespace Ticketing2.GraphQL.Web.DomainObjects;

public class KundeUser
{
    public int Id { get; set; }
    public string AspNetUserId { get; set; } //todo: als Fremdschlüssel mappen
    
    // Der Kunde kann nur Tickets haben wenn er sie gekauft hat.
    public ICollection<PurchasedTicket> PurchasedTickets { get; } = new List<PurchasedTicket>();
}