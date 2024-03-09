using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Schema.Payload;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.TokenGenerator;

namespace Ticketing.GraphQL.Web.Schema.Mutations;


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
            };
            ticketingDbContext.KundeUser.Add(kunde);
            await ticketingDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        
        //todo: das mit dem token entfernen.
        //todo: direkt das cookie senden
        var kundePayload = new KundePayload(input.Email);
        return kundePayload;
        
        
        ClaimsIdentity GenerateClaimsIdentity()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, "Kunde"),
                new(ClaimTypes.NameIdentifier, user.Id)
            };
            return new ClaimsIdentity(claims, "jwt");
        }
    }
    
    public async Task<KundePayload> LoginKunde(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        // [Service] IConfiguration configuration,
        [Service] IHttpContextAccessor httpContextAccessor,
        
        KundeLoginInput input)
    {
        var user = await ValidateInputAndGetAspNetUser();
        await LoginUser();
        
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, "login"); //todo: hier noch recherchieren ob das sinnvoll ist mit dem "login" string
        
        
        // Authentifizierungscookie erstellen
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),  // Cookie-Ablaufzeit festlegen, todo: Frage: Wieso soll ich das hier nochmal festlegen
                                                            // wenn ich das schon in startup.cs gemacht habe?
            IsPersistent = true // Cookie persistent machen; Todo: Noch kläre wie sinnvoll das ist. 
        };
        
        var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        await httpContextAccessor.HttpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties); // todo: warnung wegkriegen
        
        var kundePayload = new KundePayload(input.Email);
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