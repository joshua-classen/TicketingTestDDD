namespace Ticketing.GraphQL.Web.Schema.Payload;

public record KundePayload(
    string Email,
    string JwtToken
    );