using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;


[ExtendObjectType(Name = "Mutation")]
public class MutationVeranstalter
{
    public async Task<VeranstalterPayload> LoginVeranstalter(
        [Service] UserManager<IdentityUser> userManager,
        [Service] SignInManager<IdentityUser> signInManager,
        [Service] IConfiguration configuration,
        VeranstalterLoginInput input)
    {
        var user = await userManager.FindByEmailAsync(input.Email);
        if (user is null) throw new Exception("Benutzer nicht gefunden.");
        
        // hier könnte ich mal fragen ob der user die rolle Veranstalter hat
        
        
        var result = await signInManager.PasswordSignInAsync(user.UserName, input.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded) throw new Exception("Falsche Credentials.");
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, "Veranstalter")
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, "jwt");
        
        // Generiere das JWT-Token für den eingeloggten Benutzer
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var sectoken = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            claimsIdentity.Claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(sectoken);

        var veranstalterPayload = new VeranstalterPayload(input.Email, token);
        return veranstalterPayload;
    }
    
    public async Task<VeranstalterPayload> CreateVeranstalter(
        [Service] TicketingDbContext ticketingDbContext,
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
        
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, "Veranstalter")
        };
        var claimsIdentity = new ClaimsIdentity(claims, "jwt");
        await userManager.AddClaimsAsync(user, claims); //todo: braucht man nur hier oder auch beim login?
        
        await signInManager.SignInAsync(user, isPersistent: false);
        
        // erstelle einen Veranstalter und füge ihm die user.Id hinzu
        // und speicher ihn ab
        var veranstalter = new VeranstalterUser()
        {
            AspNetUserId = user.Id,
            Veranstaltungen = new List<Veranstaltung>()
        };
        ticketingDbContext.VeranstalterUser.Add(veranstalter);
        await ticketingDbContext.SaveChangesAsync();
        
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var sectoken = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            claimsIdentity.Claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(sectoken);
        
        var veranstalterPayload = new VeranstalterPayload(input.Email, token);
        return veranstalterPayload;
    }
}