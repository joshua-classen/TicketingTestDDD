using Microsoft.AspNetCore.Identity;

namespace Ticketing2.GraphQL.Web.Services;

public class ApplicationRole : IdentityRole<Guid>
{
    
    // überarbeiten
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}