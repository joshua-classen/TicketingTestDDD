namespace Ticketing.GraphQL.Web.DomainObjects;

public class Veranstaltung
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    [GraphQLType(typeof(UnsignedIntType))]
    public uint TicketPriceEuroCent { get; set; }
    [GraphQLType(typeof(UnsignedIntType))]
    public uint MaxAmountTickets { get; set; }
    public ICollection<PurchasedTicket> PurchasedTickets { get; } = new List<PurchasedTicket>();
}