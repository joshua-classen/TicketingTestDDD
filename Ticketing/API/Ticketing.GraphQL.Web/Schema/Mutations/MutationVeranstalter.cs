using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.EmailService;
using Ticketing.GraphQL.Web.Inputs;
using Ticketing.GraphQL.Web.Repository;
using Ticketing.GraphQL.Web.Schema.Payload;
using Ticketing.GraphQL.Web.Services;

namespace Ticketing.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationVeranstalter 
{
    // todo: überlegung. was ist wenn ich cognito dotnet sdk verwende. Der User loggt sich ein und ich bekomme ein token.
    // aber das token kann ich dem user nicht geben, weil ich das token nicht in einem cookie speichern kann.
    // bzw. ich könnte es in einem token speichern. 
    
    // dann muss ich aber die authenticator middleware umschreiben die dann einen request an cognito schickt und das token validiert.
    
    
    // todo:
    // SignUpVeranstalter legt einen neuen Veranstalter mit noch nicht validierter Email an.
    // Der Veranstalter erhält eine Email mit einem verification code.
    // Der Veranstalter wird dann den verification code eingeben und die Email wird validiert.
    // was passiert wenn er die Webseite schon schließt und dann ist ja das window mit dem verification code weg. //todo: herausfinden wie andere Webseiten das machen.
    
    
    // naja, der Veranstalter gibt seinen verification code ein und wenn alles korrekt ist, dann wird er weitergeleitet zum login, bzw. dann könnte man ihn auch eigentlich schon einloggen
    
    // der Veranstalter braucht auch noch edit funktionen um seine passwort zu ändern
    // wir brauchen eine passwort vergessen funktion
    // wir brauchen eine change email funktion
    
    
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<ChangedPasswordSuccessStatusPayload> ChangePassword(
    [Service] UserManager<IdentityUser> userManager,
    [Service] IHttpContextAccessor httpContextAccessor,
    [Service] TicketingDbContext ticketingDbContext,
    ClaimsPrincipal claimsPrincipal,
    ChangePasswordInput input)
    {
        var user = await VeranstalterRepository.GetAspNetUser(userManager, claimsPrincipal);

        await using var transaction = await ticketingDbContext.Database.BeginTransactionAsync();
        try
        {
            await VeranstalterRepository.ChangeUserPassword(userManager, user, input.OldPassword, input.NewPassword);
            await VeranstalterRepository.InvalidateAuthToken(userManager, user);
            await VeranstalterRepository.LogoutUser(httpContextAccessor);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Beim Ändern des Passworts ist ein Fehler aufgetreten. Das Passwort wurde nicht geändert.");
        }
        
        return new ChangedPasswordSuccessStatusPayload(true);
    }
    
    public async Task<VeranstalterPayload> SignUpVeranstalter(
        [Service] TicketingDbContext ticketingDbContext,
        [Service] UserManager<IdentityUser> userManager, 
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        [Service] RoleManager<IdentityRole> roleManager,
        [Service] IEmailSender emailSender,
        
        VeranstalterCreateInput input)
    {
        if (!await roleManager.RoleExistsAsync("Veranstalter"))
        {
            await roleManager.CreateAsync(new IdentityRole("Veranstalter"));
        }
        
        var user = new IdentityUser() { UserName = input.Email, Email = input.Email};
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
            
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user); //todo: ist noch zu lange. muss gekürzt werden.
            try
            {
                await emailSender.SendEmailAsync(user.Email, "Ihr Verfication code - classenTicketing.com", "Ihr verification code ist: " + code);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Beim versenden der verification Email ist ein Fehler aufgetreten.");
            }
            
            var veranstalter = new VeranstalterUser()
            {
                AspNetUserId = user.Id,
                Veranstaltungen = new List<Veranstaltung>()
            };
            ticketingDbContext.VeranstalterUser.Add(veranstalter);
            await ticketingDbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        return new VeranstalterPayload(input.Email);
        
        
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
    
    public async Task<LogoutUserSuccessStatusPayload> LogoutVeranstalter(
        [Service] UserManager<IdentityUser> userManager,
        [Service] TicketingDbContext ticketingDbContext,
        [Service] IHttpContextAccessor httpContextAccessor,
        ClaimsPrincipal claimsPrincipal
        )
    {
        var user = await VeranstalterRepository.GetAspNetUser(userManager, claimsPrincipal);
        
        await using var transaction = await ticketingDbContext.Database.BeginTransactionAsync();

        try
        {
            await VeranstalterRepository.InvalidateAuthToken(userManager, user);
            await VeranstalterRepository.LogoutUser(httpContextAccessor);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Es ist ein Fehler beim Ausloggen passiert. Der User wurde nicht ausgeloggt.");
        }
        
        return new LogoutUserSuccessStatusPayload(true);
    }
    
    public async Task<VeranstalterPayload> LoginVeranstalter(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IHttpContextAccessor httpContextAccessor,
        
        VeranstalterLoginInput input)
    {
        if (input.Email is null || input.Password is null)
        {
            throw new Exception("Email oder Passwort ist null.");
        }

        var user = await VeranstalterRepository.GetAspNetUserByEmail(userManager, input.Email);
        await VeranstalterRepository.LoginUser(signInManager, user, input.Password);
        
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, "login"); 
        
        var authProperties = new AuthenticationProperties
        {
            // ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1), // Cookie-Ablaufzeit festlegen wird hiermit überschrieben
            IsPersistent = true // Cookie persistent machen; Todo: Noch kläre wie sinnvoll das ist. 
        };
        
        var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Es ist ein Fehler beim ausloggen passiert. Der User wurde nicht ausgeloggt.");
        }
        await context.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        
        return new VeranstalterPayload(input.Email);
    }
    
    
    //todo: implement VerifyVeranstalterEmail Method to verify the email of the Veranstalter

    // public async Task<VerifyEmailSuccessStatusPayload> VerifyVeranstalterEmail(
    //     VeranstalterEmailVerificationInput input,
    //     
    //     )
    // {
    //     
    //     
    // }
    
    
    //todo: implement Veranstalter komplett löschen
    
    
    
}