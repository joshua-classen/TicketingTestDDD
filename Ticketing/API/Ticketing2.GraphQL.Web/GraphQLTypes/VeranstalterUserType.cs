using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.GraphQLTypes;

public class VeranstalterUserType : ObjectType<VeranstalterUser>
{
    protected override void Configure(IObjectTypeDescriptor<VeranstalterUser> descriptor)
    {
        base.Configure(descriptor);
        
        descriptor.BindFieldsExplicitly();
        descriptor.Field(veranstalterUser => veranstalterUser.Id);
        descriptor.Field(veranstalterUser => veranstalterUser.AspNetUserId).Ignore();
        descriptor.Field(veranstalterUser => veranstalterUser.Veranstaltungen);
    }
}