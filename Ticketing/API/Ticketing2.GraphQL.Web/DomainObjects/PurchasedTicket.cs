namespace Ticketing2.GraphQL.Web.DomainObjects;

public class PurchasedTicket
{
    public int Id { get; set; } // muss glaube ich nicht public sein: https://learn.microsoft.com/en-us/ef/core/modeling/relationships 
    public uint TicketNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public uint TicketPriceEuroCent { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
}