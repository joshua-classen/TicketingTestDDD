using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.GraphQLTypes;

public class PurchasedTicketType : ObjectType<PurchasedTicket>
{
    protected override void Configure(IObjectTypeDescriptor<PurchasedTicket> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(purchasedTicket => purchasedTicket.Id);
        descriptor.Field(purchasedTicket => purchasedTicket.KundeUser).Ignore();
        descriptor.Field(purchasedTicket => purchasedTicket.TicketNumber);
        descriptor.Field(purchasedTicket => purchasedTicket.PurchaseDate);
        descriptor.Field(purchasedTicket => purchasedTicket.TicketPriceEuroCent);
        descriptor.Field(purchasedTicket => purchasedTicket.Veranstaltung); // hier brauche ich vll einen Resolver oder sowas???

    }
}