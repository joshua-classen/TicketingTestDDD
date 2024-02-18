using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing2.GraphQL.Web.DomainObjects;

public class KundeUser
{
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // https://stackoverflow.com/questions/23081096/entity-framework-6-guid-as-primary-key-cannot-insert-the-value-null-into-column
    public Guid Id { get; set; }
    public Guid AspNetUserId { get; set; } //todo: als Fremdschlüssel mappen
    
    // Der Kunde kann nur Tickets haben wenn er sie gekauft hat.
    public ICollection<PurchasedTicket> PurchasedTickets { get; } = new List<PurchasedTicket>();
}