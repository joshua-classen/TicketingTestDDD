using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;
using Ticketing2.GraphQL.Web.TokenGenerator;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationKunde 
{
    public async Task<KundePayload> CreateKunde(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        [Service] RoleManager<IdentityRole> roleManager,

        KundeCreateInput input)
    {
        if (!await roleManager.RoleExistsAsync("Kunde"))
        {
            await roleManager.CreateAsync(new IdentityRole("Kunde"));
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

            await userManager.AddToRoleAsync(user, "Kunde");
            await userManager.AddClaimsAsync(user, claimsIdentity.Claims); 
            await signInManager.SignInAsync(user, isPersistent: false);

            var kunde = new KundeUser()
            {
                AspNetUserId = user.Id,
                Tickets = new List<Ticket>()
            };
            ticketingDbContext.KundeUser.Add(kunde);
            await ticketingDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        var token = JwtTokenGenerator.GenerateToken(configuration, claimsIdentity);
        var kundePayload = new KundePayload(input.Email, token);
        return kundePayload;
        
        
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
    
    
    
    public async Task<KundePayload> LoginKunde(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        
        KundeLoginInput input)
    {
        var user = await ValidateInputAndGetAspNetUser();
        await LoginUser();
        
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, "jwt");
        
        var token = JwtTokenGenerator.GenerateToken(configuration, claimsIdentity);
        var kundePayload = new KundePayload(input.Email, token); // todo: wie speichert man das im cookie ab?
        return kundePayload;
        
        
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