using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Schema.Payload;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.TokenGenerator;

namespace Ticketing.GraphQL.Web.Schema.Mutations;


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
        
        
        //todo: direkt das cookie senden
        var veranstalterPayload = new VeranstalterPayload(input.Email);
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
        // [Service] IConfiguration configuration,
        [Service] IHttpContextAccessor httpContextAccessor,
        
        VeranstalterLoginInput input)
    {
        var user = await ValidateInputAndGetAspNetUser();
        await LoginUser();
        
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, "login"); 
        
        //todo: JwtTokenGenerator deprecated machen bzw. löschen
        
        // var token = JwtTokenGenerator.GenerateToken(configuration, claimsIdentity);
        // Authentifizierungscookie erstellen
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1), // Cookie-Ablaufzeit festlegen, todo: Frage: Wieso soll ich das hier nochmal festlegen
                                                           // wenn ich das schon in startup.cs gemacht habe?
            IsPersistent = true // Cookie persistent machen; Todo: Noch kläre wie sinnvoll das ist. 
        };
        
        var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        await httpContextAccessor.HttpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties); // todo: warnung wegkriegen
        
        
        // Bei REST-APIs würde ich hier ein 200 OK zurückgeben.
        // was mache ich aber bei einer graphql Mutation?
        return new VeranstalterPayload(input.Email);
        
        
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