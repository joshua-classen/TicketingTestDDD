namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record VeranstalterPayload(
    string Email,
    string JwtToken
    );