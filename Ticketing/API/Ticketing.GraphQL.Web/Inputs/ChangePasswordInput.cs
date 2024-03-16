namespace Ticketing.GraphQL.Web.Inputs;

public record ChangePasswordInput(string OldPassword, string NewPassword);