namespace Ticketing2.GraphQL.Web.DomainObjects;

public class VeranstalterUser
{
    public int Id { get; set; }
    public string AspNetUserId { get; set; }
    public ICollection<Veranstaltung> Veranstaltungen { get; set; }
}