using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Inputs;
using Ticketing2.GraphQL.Web.Schema.Payload;
using Ticketing2.GraphQL.Web.Services;
using Ticketing2.GraphQL.Web.TokenGenerator;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationVeranstalter
{
    public async Task<VeranstalterPayload> CreateVeranstalter(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager, 
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        [Service] RoleManager<IdentityRole> roleManager,
        
        VeranstalterCreateInput input)
    {
        if (!await roleManager.RoleExistsAsync("Veranstalter"))
        {
            await roleManager.CreateAsync(new IdentityRole("Veranstalter"));
        }
        
        var user = new IdentityUser() { UserName = input.Email };
        var claimsIdentity = GenerateClaimsIdentity();

        await using var transaction = await ticketingDbContext.Database.BeginTransactionAsync();
        try
        {
            var result = await userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                var msg = result.Errors.Select(e => e.Description.ToString()).Aggregate((a, b) => a + ", " + b);
                throw new Exception(msg);
            }

            await userManager.AddToRoleAsync(user, "Veranstalter");
            await userManager.AddClaimsAsync(user, claimsIdentity.Claims); 
            await signInManager.SignInAsync(user, isPersistent: false);

            var veranstalter = new VeranstalterUser()
            {
                AspNetUserId = user.Id,
                Veranstaltungen = new List<Veranstaltung>()
            };
            ticketingDbContext.VeranstalterUser.Add(veranstalter);
            await ticketingDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        var token = JwtTokenGenerator.GenerateToken(configuration, claimsIdentity);
        var veranstalterPayload = new VeranstalterPayload(input.Email, token);
        return veranstalterPayload;
        
        
        ClaimsIdentity GenerateClaimsIdentity()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, "Veranstalter"),
                new(ClaimTypes.NameIdentifier, user.Id)
            };
            return new ClaimsIdentity(claims, "jwt");
        }
    }
    
    public async Task<VeranstalterPayload> LoginVeranstalter(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        
        VeranstalterLoginInput input)
    {
        var user = await ValidateInputAndGetAspNetUser();
        await LoginUser();
        
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, "jwt");
        
        var token = JwtTokenGenerator.GenerateToken(configuration, claimsIdentity);
        var veranstalterPayload = new VeranstalterPayload(input.Email, token); // todo: wie speichert man das im cookie ab?
        return veranstalterPayload;
        
        
        async Task<IdentityUser> ValidateInputAndGetAspNetUser()
        {
            if (input.Email is null || input.Password is null)
            {
                throw new Exception("Email oder Passwort ist null.");
            }
        
            var myUser = await userManager.FindByNameAsync(input.Email);
            if (myUser is null)
            {
                throw new Exception("Benutzer nicht gefunden.");
            }

            return myUser;
        }
        async Task LoginUser()
        {
            var result = await signInManager.PasswordSignInAsync(
                user.UserName ?? throw new Exception("user.UserName ist null."),
                input.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid Credentials.");
            }
        }
    }
}