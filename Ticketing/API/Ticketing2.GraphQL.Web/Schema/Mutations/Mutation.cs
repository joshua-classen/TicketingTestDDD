using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;

// public interface IAuthenticationService
// {
//     Task<Veranstalter> AuthenticateAsync(string email, string password);
//     string GenerateToken(Veranstalter user);
// }

public class AuthenticationService : IAuthenticationService
{
    // Implementiere die Authentifizierungslogik hier
    // Du könntest ASP.NET Identity oder eine andere Bibliothek verwenden

    public async Task<Veranstalter> AuthenticateAsync(string email, string password)
    {
        // Beispielcode für die Authentifizierung
        // Hier musst du die Logik an deine Anforderungen anpassen
        
        // get user with email from db
        // compare hashed and salted password with input password
        // wenn das korrekt ist, dann hole dir die Id von dem Veranstalter
        
        // hole dann den Veranstalter aus der Datenbank und gebe ihn zurück
        

        return null; // Authentifizierung fehlgeschlagen
    }

    public string GenerateToken(Veranstalter user)
    {
        // Implementiere die Token-Generierungslogik hier
        // Du könntest JWT oder eine andere Methode verwenden
        return "dummyAuthToken";
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        throw new NotImplementedException();
    }

    public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
}


public class Mutation
{
    
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
    
    
    
    // sollte ich hier in der mutation eine methode login erstellen?
    // weiß ich nicht.
    //[Authorize]
    public async Task<Veranstalter> UserLogin(VeranstalterLoginInput input, [Service] IAuthenticationService authService)
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
    
    // das hier ist der register Veranstalter
    public async Task<VeranstalterPayload> CreateVeranstalter([FromServices] TicketingDbContext context, VeranstalterCreateInput input)
    {
        // hash and salt password
        // var hashedSaltedPassword = BCrypt.Net.BCrypt.HashPassword(input.Password);
        var hashedSaltedPassword = input.Password;
        
        var veranstalter = new Veranstalter
        {
            Name = input.Name,
            Email = input.Email,
            hashPasswort = hashedSaltedPassword
        };
        context.Veranstalter.Add(veranstalter);
        await context.SaveChangesAsync();
        return new VeranstalterPayload(veranstalter);
        // return veranstalter;
    }
}