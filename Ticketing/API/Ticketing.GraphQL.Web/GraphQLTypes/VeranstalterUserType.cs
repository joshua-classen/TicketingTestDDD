using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.GraphQLTypes;

public class VeranstalterUserType : ObjectType<VeranstalterUser>
{
    protected override void Configure(IObjectTypeDescriptor<VeranstalterUser> descriptor)
    {
        base.Configure(descriptor);
        
        descriptor.BindFieldsExplicitly();
        descriptor.Field(veranstalterUser => veranstalterUser.Id);
        descriptor.Ignore(veranstalterUser => veranstalterUser.AspNetUserId);
        descriptor.Field(veranstalterUser => veranstalterUser.Veranstaltungen);
    }
}