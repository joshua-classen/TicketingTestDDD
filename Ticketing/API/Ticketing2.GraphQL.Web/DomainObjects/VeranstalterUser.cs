namespace Ticketing2.GraphQL.Web.DomainObjects;

public class VeranstalterUser
{
    public Guid Id { get; set; }
    public Guid AspNetUserId { get; set; }
    public ICollection<Veranstaltung> Veranstaltungen { get; set; }
}