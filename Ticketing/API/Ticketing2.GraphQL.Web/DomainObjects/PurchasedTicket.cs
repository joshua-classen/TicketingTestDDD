namespace Ticketing2.GraphQL.Web.DomainObjects;

public class PurchasedTicket
{
    public Guid Id { get; set; } // muss glaube ich nicht public sein: https://learn.microsoft.com/en-us/ef/core/modeling/relationships 
    public KundeUser KundeUser { get; set; } = null!;
    [GraphQLType(typeof(UnsignedIntType))]
    public uint TicketNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    [GraphQLType(typeof(UnsignedIntType))]
    public uint TicketPriceEuroCent { get; set; }
    public Veranstaltung Veranstaltung { get; set; } = null!;
}