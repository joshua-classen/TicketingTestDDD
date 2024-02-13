namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public record VeranstalterCreateInput(
    string Name,
    string Email,
    string Password
    );