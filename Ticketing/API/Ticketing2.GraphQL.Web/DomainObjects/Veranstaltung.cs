namespace Ticketing2.GraphQL.Web.DomainObjects;

public class Veranstaltung
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<Ticket> Tickets { get; set; }
    // public uint MaxAmountTickets { get; set; }
}