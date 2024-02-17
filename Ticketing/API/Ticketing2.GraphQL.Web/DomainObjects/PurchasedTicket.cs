namespace Ticketing2.GraphQL.Web.DomainObjects;

public class PurchasedTicket
{
    public int Id { get; set; } // muss glaube ich nicht public sein: https://learn.microsoft.com/en-us/ef/core/modeling/relationships 
    public KundeUser KundeUser { get; set; } = null!;
    public int TicketNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int TicketPriceEuroCent { get; set; } // ich glaube der hat ein problem mit uint // hot chocolate hat ein problem mit uint. // todo: lösen
    public Veranstaltung Veranstaltung { get; set; } = null!;
}