using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;






namespace Ticketing2.GraphQL.Web.Schema.Mutations;

// 1. Wir injezieren alle Services in den Methoden
// ODER
// 2. Wir injezieren alle Services in den Konstruktor und speichern sie in privaten Feldern

public class AppSettings
{
    public string Secret { get; set; }
}

public class Mutation
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IOptions<AppSettings> _appSettings; // todo: entfernen

    public Mutation(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _appSettings = appSettings;
    }
    
    [Authorize(Roles = ["Veranstalter"])]
    public async Task<Veranstalter> CreateVeranstaltung(VeranstalterLoginInput input, [Service] IAuthenticationService authService)
    {
        
        Console.WriteLine("Hier");
        await Task.Delay(5);
        
        var veranstalter = new Veranstalter()
        {
            Email = "asdf@test.com",
            Name = "torsten",
            hashPasswort = "adsfasdifhaskjdfhaksdf",
            Id = 42
        };
        return veranstalter;
    }
    
    public async Task<VeranstalterPayload> VeranstalterLogin(VeranstalterLoginInput input)
    {
        Console.WriteLine("Hier");
        await Task.Delay(5);
        
        
        // Überprüfen der Anmeldeinformationen
        if (!Authenticate(input.Email, input.Password))
            throw new UnauthorizedAccessException(); // wahrscheinlich noch nicht korrekte Fehlermeldung
        
        
        // Rollenzuweisung für den Veranstalter
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, input.Email),
            new Claim(ClaimTypes.Role, "Veranstalter")
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HierDeinSuperGeheimesTokenKey"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "deineWebsite.com",
            audience: "deineWebsite.com",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );
        
        var veranstalterPayload = new VeranstalterPayload(input.Email, token.ToString());
        return veranstalterPayload;
    }
    
    private bool Authenticate(string email, string password)
    {
        // Überprüfen der Anmeldeinformationen gegen gespeicherte Anmeldeinformationen
        // if (_veranstalterCredentials.TryGetValue(email, out var storedPassword))
        // {
        //     // Hier solltest du deine Authentifizierungslogik einfügen, z.B. überprüfen einer Datenbank oder eines externen Authentifizierungsdienstes
        //     return password == storedPassword;
        // }
        return false;
    }
    
    public async Task<VeranstalterPayload> CreateVeranstalter(
        [Service] TicketingDbContext context,
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
    
    public string GenerateJwtToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        // das muss ich doch garnicht mehr weil ich den key schon in startup.cs konfiguriert habe
        var key = Encoding.ASCII.GetBytes("hier mein super geheimer key der auch meagagaggaga langs ist blabalbalba");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}