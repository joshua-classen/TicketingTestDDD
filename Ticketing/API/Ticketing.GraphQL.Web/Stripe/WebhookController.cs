using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ticketing.GraphQL.Web.Stripe;

/// <summary>
/// Instruction:
/// Download Cli and
/// stripe login
///
/// Forward events to webhook
/// stripe listen --forward-to localhost:4242/webhook
///
/// Trigger events with the cli for e.g.
/// stripe trigger payment_intent.succeeded
/// </summary>


// das hier ist ein seperater endpunkt der von stripe aufgerufen wird um mir infos über die zahlung zu geben.
// ich kann hier später bestimmt noch einstellen das er nur anfragen von stripe akzeptiert.
[Route("webhook")]
[ApiController]
public class WebhookController(IStripeEnpointWebHookSecret stripeEnpointWebHookSecret) : ControllerBase
{
    private readonly string _endpointSecret = stripeEnpointWebHookSecret.Secret;

    // This is your Stripe CLI webhook secret for testing your endpoint locally.
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _endpointSecret);

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}