using Stripe;

namespace Ticketing.GraphQL.Web.Stripe;

public static class StripeService
{
    public static string StripeGenericPaymentIntent()
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = 1099,
            Currency = "eur",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
        };
        var service = new PaymentIntentService();
        var myPaymentIntent = service.Create(options);
        // sollte ich das PaymentIntent in meiner DB speichern?


        var clientSecret = myPaymentIntent.ClientSecret;
        return clientSecret;
    
        // dieses client geheimnis wird dann im frontend benutzt, um die zahlung zu bestätigen. Der Client fragt das über meine api an. Etwa so

        
        // client:
        // Und dann rufen Sie das Client-Geheimnis mit JavaScript auf der Client-Seite ab:
        // enthält einen iFrame
        // (async () => {
        //     const response = await fetch('/secret');
        //     const {client_secret: clientSecret} = await response.json();
        //     // Render the form using the clientSecret
        // })();

        
        // Frage: Wie erfahre ich jetzt ob der Kunde die Zahlung erfolgreich abgeschlossen hat?
        // https://docs.stripe.com/payments/payment-intents/verifying-status#webhooks
        // Nachdem der Client die Zahlung bestätigt hat, wird empfohlen, dass
        // Ihr Server Webhooks überwacht, um zu erkennen, wann die Zahlung erfolgreich abgeschlossen wird oder fehlschlägt.

        // Um ein Webhook-Ereignis zu verarbeiten, erstellen Sie eine Route auf Ihrem Server
        // und konfigurieren Sie einen entsprechenden Webhook-Endpoint im Dashboard. 
    }
    
    
    /*
     * Best practices paymentIntent :
     * - PaymentIntent erstellen wenn ich den Betrag kennen.
     * 
     * - Wenn der Zahlungsvorgang unterbrochen und später wiederaufgenommen wird,
     *   sollten Sie versuchen, denselben PaymentIntent erneut zu verwenden, statt einen neuen zu erstellen.
     *
     * - Denken Sie daran, einen Idempotenzschlüssel anzugeben, um die Erstellung doppelter
     *   PaymentIntents für denselben Kauf zu verhindern.
     * 
     */
    
    
    public static void StripeTest()
    {
        // stripeApi key im startup.cs gesetzt.

        var optionsProduct = new ProductCreateOptions
        {
            Name = "Starter Subscription",
            Description = "$12/Month subscription",
        };
        var serviceProduct = new ProductService();
        Product product = serviceProduct.Create(optionsProduct);
        Console.Write("Success! Here is your starter subscription product id: {0}\n", product.Id);

        var optionsPrice = new PriceCreateOptions
        {
            UnitAmount = 1200,
            Currency = "usd",
            Recurring = new PriceRecurringOptions
            {
                Interval = "month",
            },
            Product = product.Id
        };
        var servicePrice = new PriceService();
        Price price = servicePrice.Create(optionsPrice);
        Console.Write("Success! Here is your starter subscription price id: {0}\n", price.Id);
    }
}