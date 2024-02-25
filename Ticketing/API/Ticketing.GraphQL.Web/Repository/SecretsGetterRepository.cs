using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Ticketing.GraphQL.Web.Repository;

public class SecretsGetterRepository
{
    public static async Task<string> GetStripeSkTestSecret()
    {
        string secretName = "TicketingDDD_Stripe__StripeConfigurationApiKey";
        string region = "eu-central-1";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch (Exception e)
        {
            // For a list of the exceptions thrown, see
            // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            throw e;
        }
        string secret = response.SecretString;

        return secret;
    }
}