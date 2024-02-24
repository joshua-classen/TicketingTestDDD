using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.GraphQLTypes;

public class PurchasedTicketType : ObjectType<PurchasedTicket>
{
    protected override void Configure(IObjectTypeDescriptor<PurchasedTicket> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(purchasedTicket => purchasedTicket.Id);
        descriptor.Ignore(purchasedTicket => purchasedTicket.KundeUser);
        descriptor.Field(purchasedTicket => purchasedTicket.TicketNumber);
        descriptor.Field(purchasedTicket => purchasedTicket.PurchaseDate);
        descriptor.Field(purchasedTicket => purchasedTicket.TicketPriceEuroCent);
        
        descriptor.Field(purchasedTicket => purchasedTicket.Veranstaltung);
    }
}