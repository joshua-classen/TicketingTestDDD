namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record KundePayload(
    string Email,
    string JwtToken
    );