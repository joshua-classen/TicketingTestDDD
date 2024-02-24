using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.GraphQLTypes;

public class KundeUserType : ObjectType<KundeUser>
{
    protected override void Configure(IObjectTypeDescriptor<KundeUser> descriptor)
    {
        base.Configure(descriptor);
        
        descriptor.BindFieldsExplicitly();
        descriptor.Field(kundeUser => kundeUser.Id);
        descriptor.Ignore(kundeUser => kundeUser.AspNetUserId);
        descriptor.Field(kundeUser => kundeUser.PurchasedTickets);
    }
}