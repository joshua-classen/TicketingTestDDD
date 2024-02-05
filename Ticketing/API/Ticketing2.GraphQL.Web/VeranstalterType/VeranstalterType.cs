using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.VeranstalterType;

public class VeranstalterType : ObjectType<Veranstalter>
{
    protected override void Configure(IObjectTypeDescriptor<Veranstalter> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        
        descriptor.Field(veranstalter => veranstalter.Name);
        descriptor.Field(veranstalter => veranstalter.Email);
        descriptor.Ignore(veranstalter => veranstalter.hashPasswort);
    }
}