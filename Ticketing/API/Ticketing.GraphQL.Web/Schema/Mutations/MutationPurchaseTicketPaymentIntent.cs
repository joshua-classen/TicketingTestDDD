using HotChocolate.Authorization;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Schema.Payload;
using Ticketing.GraphQL.Web.Stripe;

namespace Ticketing.GraphQL.Web.Schema.Mutations;

[ExtendObjectType(Name = "Mutation")]
public class MutationPurchaseTicketPaymentIntent
{
    [Authorize(Roles = ["Kunde"])]
    public async Task<StripeClientSecretPayload> CreatePaymentIntent(
        // hier fehlen noch parameter
        BuyTicketCreateInput input
    )
    {
        await Task.Delay(10);
        var clientSecret = StripeService.StripeGenericPaymentIntent();
        
        return new StripeClientSecretPayload(clientSecret);
    }
}
