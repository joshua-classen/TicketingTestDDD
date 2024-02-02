namespace Ticketing2.GraphQL.Web.DTOs;

public class VeranstalterDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
}