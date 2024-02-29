using Ticketing.GraphQL.Web.Schema.Payload;

namespace Ticketing.GraphQL.Web.GraphQLTypes;

public class StripeClientSecretPayloadType : ObjectType<StripeClientSecretPayload>
{
    protected override void Configure(IObjectTypeDescriptor<StripeClientSecretPayload> descriptor)
    {
        base.Configure(descriptor);
        descriptor.Field(stripeClientSecretPayload => stripeClientSecretPayload.ClientSecret);
    }
}