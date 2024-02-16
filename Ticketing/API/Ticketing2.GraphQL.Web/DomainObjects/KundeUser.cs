namespace Ticketing2.GraphQL.Web.DomainObjects;

public class KundeUser
{
    public int Id { get; set; }
    public string AspNetUserId { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}