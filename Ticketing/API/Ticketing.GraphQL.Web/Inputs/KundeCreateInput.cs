namespace Ticketing.GraphQL.Web.Inputs;

public record KundeCreateInput(
    string Email,
    string Password
    );