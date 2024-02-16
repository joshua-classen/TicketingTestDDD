namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record KundeCreateInput(
    string Email,
    string Password
    );