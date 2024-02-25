using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Ticketing.GraphQL.Web.Stripe;

namespace StripeExampleApi.Controllers;

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
public class WebhookController : ControllerBase
{
    
    // wie kann ich hier objekte injecten?
    public WebhookController(IStripeEnpointWebHookSecret stripeEnpointWebHookSecret)
    {
        _endpointSecret = stripeEnpointWebHookSecret.Secret;
    }
    
    
    // This is your Stripe CLI webhook secret for testing your endpoint locally.
    
    // das ist hier noch blöd. Ich möchte das die variable schon in startup geladen wird.
    private readonly string _endpointSecret;// = Environment.GetEnvironmentVariable("TicketingDDD_Stripe__StripeWebhookSecretTestMode_Localhost"); 

    [HttpPost]
    public async Task<IActionResult> Index()
    {
        Console.WriteLine("Im WebhookController");
        Console.WriteLine(_endpointSecret);
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