namespace Ticketing2.GraphQL.Web.Schema.Queries;

public record VeranstalterType(
    Guid Id,
    string Name,
    string Email,
    string HashedPassword);
    