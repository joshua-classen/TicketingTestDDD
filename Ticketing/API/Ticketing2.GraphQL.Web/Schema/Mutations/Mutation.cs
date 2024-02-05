using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Mutations;

public interface IAuthenticationService
{
    Task<Veranstalter> AuthenticateAsync(string email, string password);
    string GenerateToken(Veranstalter user);
}

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
}


public class Mutation
{
    // sollte ich hier in der mutation eine methode login erstellen?
    // ja weil ich ja auch das token speichern muss. 
    // public async Task<AuthPayload> UserLogin(VeranstalterLoginInput input, [Service] IAuthenticationService authService)
    // {
    //     // Überprüfe Benutzeranmeldeinformationen und generiere Token
    //     // Hier musst du die Authentifizierungslogik implementieren
    //
    //     // Dummy-Code für Beispielzwecke
    //     var user = await authService.AuthenticateAsync(input.Email, input.Password);
    //     
    //     if (user == null)
    //     {
    //         // Authentifizierung fehlgeschlagen
    //         throw new GraphQLRequestException(ErrorBuilder.New()
    //             .SetMessage("Anmeldung fehlgeschlagen. Überprüfen Sie Ihre Anmeldeinformationen.")
    //             .SetCode("AUTH_ERROR")
    //             .Build());
    //     }
    //
    //     var token = authService.GenerateToken(user);
    //
    //     return new AuthPayload { User = user, Token = token };
    // }
    
    
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