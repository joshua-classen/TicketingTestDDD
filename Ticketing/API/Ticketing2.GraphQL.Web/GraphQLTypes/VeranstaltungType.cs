using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.GraphQLTypes;

public class VeranstaltungType : ObjectType<Veranstaltung>
{
    protected override void Configure(IObjectTypeDescriptor<Veranstaltung> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(veranstaltung => veranstaltung.Id);
        descriptor.Field(veranstaltung => veranstaltung.Name);
        descriptor.Field(veranstaltung => veranstaltung.TicketPriceEuroCent);
        descriptor.Field(veranstaltung => veranstaltung.MaxAmountTickets);
        descriptor.Field(veranstalung => veranstalung.PurchasedTickets);
    }
}