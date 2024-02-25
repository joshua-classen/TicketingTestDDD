using Stripe;

namespace Ticketing.GraphQL.Web.Stripe;

public static class StripeService
{
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