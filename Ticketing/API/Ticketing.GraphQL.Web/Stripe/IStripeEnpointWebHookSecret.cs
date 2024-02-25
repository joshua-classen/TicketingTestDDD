namespace Ticketing.GraphQL.Web.Stripe;

public interface IStripeEnpointWebHookSecret
{
    string Secret { get; }
}