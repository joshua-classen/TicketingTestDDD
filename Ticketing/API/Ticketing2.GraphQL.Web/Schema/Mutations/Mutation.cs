using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public class Mutation
{
    // private properties entfernen da alle Methoden die Services injeziert kriegen.
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    
    // todo: Konstruktor eventuell entfernen
    public Mutation(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [Authorize(Roles = ["Veranstalter"])] // das funktioniert noch nicht.
    // [Authorize] // das funktioniert mit [Authorize]
    public async Task<Veranstaltung> CreateVeranstaltung(
        [Service] TicketingDbContext context,
        
        VeranstaltungCreateInput input
        )
    {
        var veranstaltung = new Veranstaltung()
        {
            Name = input.Name
        };
        context.Veranstaltung.Add(veranstaltung);
        await context.SaveChangesAsync();

        return veranstaltung;
    }
    
    public async Task<VeranstalterPayload> LoginVeranstalter(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        VeranstalterLoginInput input)
    {
        var user = await userManager.FindByEmailAsync(input.Email);

        if (user is null)
        {
            throw new Exception("Benutzer nicht gefunden.");
        }
        
        var result = await signInManager.PasswordSignInAsync(user.UserName, input.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new Exception("Falsches Passwort.");
        }

        // Generiere das JWT-Token für den eingeloggten Benutzer
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var sectoken = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(sectoken);

        var veranstalterPayload = new VeranstalterPayload(input.Email, token);
        return veranstalterPayload;
        
        ///////
        // Rollenzuweisung für den Veranstalter
        // var claims = new List<Claim>
        // {
        //     new Claim(ClaimTypes.Name, input.Email),
        //     new Claim(ClaimTypes.Role, "Veranstalter")
        // };
    }
    
    public async Task<VeranstalterPayload> CreateVeranstalter(
        [Service] UserManager<IdentityUser> userManager, 
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        [Service] RoleManager<IdentityRole> roleManager,
        
        VeranstalterCreateInput input)
    {
        var user = new IdentityUser() { UserName = input.Name, Email = input.Email };
        var result = await userManager.CreateAsync(user, input.Password);
        
        // todo: Auslagern in Startup.cs
        if (!await roleManager.RoleExistsAsync("Veranstalter"))
        {
            await roleManager.CreateAsync(new IdentityRole("Veranstalter"));
        }
        
        if (!result.Succeeded)
        {
            var msg = result.Errors.Select(e => e.Description.ToString()).Aggregate((a, b) => a + ", " + b);
            throw new Exception(msg);
        }
        
        // ich glaube das muss man hier nur im createVeranstalter machen und nicht im loginVeranstalter
        await userManager.AddToRoleAsync(user, "Veranstalter"); 
        
        await signInManager.SignInAsync(user, isPersistent: false);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var sectoken = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(sectoken);
        
        var veranstalterPayload = new VeranstalterPayload(input.Email, token);
        return veranstalterPayload;
    }
}