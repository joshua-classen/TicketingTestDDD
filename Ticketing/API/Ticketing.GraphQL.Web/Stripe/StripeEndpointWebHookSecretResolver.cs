namespace Ticketing.GraphQL.Web.Stripe;

public class StripeEndpointWebHookSecretResolver : IStripeEnpointWebHookSecret
{
    public string Secret { get; }
    
    public StripeEndpointWebHookSecretResolver()
    {
        Secret = Environment.GetEnvironmentVariable("TicketingDDD_Stripe__StripeWebhookSecretTestMode_Localhost")
                  ?? throw new Exception("TicketingDDD_Stripe__StripeWebhookSecretTestMode_Localhost not found in environment variables");
    }
}