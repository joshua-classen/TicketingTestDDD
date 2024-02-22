using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.GraphQLTypes;

public class PurchasedTicketType : ObjectType<PurchasedTicket>
{
    protected override void Configure(IObjectTypeDescriptor<PurchasedTicket> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(purchasedTicket => purchasedTicket.Id);
        descriptor.Field(purchasedTicket => purchasedTicket.KundeUser).Ignore(); // todo: das wird hier nicht ignoriert. Warum nicht?
        // descriptor.Ignore(purchasedTicket => purchasedTicket.KundeUser).Ignore(); // todo: funktioniert auch nicht. Warum nicht?
        descriptor.Field(purchasedTicket => purchasedTicket.TicketNumber);
        descriptor.Field(purchasedTicket => purchasedTicket.PurchaseDate);
        descriptor.Field(purchasedTicket => purchasedTicket.TicketPriceEuroCent);
        
        // todo: fix:
        descriptor.Field(purchasedTicket => purchasedTicket.Veranstaltung); // hier brauche ich vll einen Resolver oder sowas???
    }
}