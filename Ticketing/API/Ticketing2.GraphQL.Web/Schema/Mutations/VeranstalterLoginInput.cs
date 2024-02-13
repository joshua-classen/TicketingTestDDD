namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record VeranstalterLoginInput(
    string Email,
    string Password
    );