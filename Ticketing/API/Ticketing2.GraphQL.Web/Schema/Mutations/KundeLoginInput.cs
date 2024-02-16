namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record KundeLoginInput(
    string Email,
    string Password
    );