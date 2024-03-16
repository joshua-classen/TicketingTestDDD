using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.GraphQL.Web.DomainObjects;
using Ticketing.GraphQL.Web.Services;

namespace Ticketing.GraphQL.Web.Repository;

public static class VeranstalterRepository
{
    public static async Task<VeranstalterUser> GetVeranstalter(
        TicketingDbContext ticketingDbContext,
        UserManager<IdentityUser> userManager,
        
        ClaimsPrincipal claimsPrincipal)
    {
        var aspNetUser = await userManager.GetUserAsync(claimsPrincipal);
        if (aspNetUser is null)
        {
            throw new Exception("AspNetUser nicht gefunden.");
        }
        
        var veranstalter = ticketingDbContext.VeranstalterUser
            .Include(vu => vu.Veranstaltungen)
            .ThenInclude(v => v.PurchasedTickets)
            .FirstOrDefault(vu => vu.AspNetUserId == aspNetUser.Id);
        if (veranstalter is null)
        {
            throw new Exception("Veranstalter nicht gefunden.");
        }

        return veranstalter;
    }
    
    public static async Task<IdentityUser> GetAspNetUser(UserManager<IdentityUser> userManager, ClaimsPrincipal claimsPrincipal)
    {
        var aspNetUser = await userManager.GetUserAsync(claimsPrincipal);
        if (aspNetUser is null)
        {
            throw new Exception("AspNetUser nicht gefunden.");
        }

        return aspNetUser;
    }
    
    public static async Task ChangeUserPassword(UserManager<IdentityUser> userManager, IdentityUser user, string oldPassword, string newPassword)
    {
        var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            var msg = result.Errors.Select(e => e.Description.ToString()).Aggregate((a, b) => a + ", " + b);
            throw new Exception("Passwort konnte nicht geändert werden: " + msg);
        }
    }
    
    public static async Task InvalidateAuthToken(UserManager<IdentityUser> userManager, IdentityUser user)
    {
        var result = await userManager.RemoveAuthenticationTokenAsync(user, CookieAuthenticationDefaults.AuthenticationScheme, "myAuthCookie");
        if (!result.Succeeded)
        {
            throw new Exception("Token konnte nicht aus dem userManager entfernt werden.");
        }
    }
    
    public static async Task LogoutUser(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new Exception("Es ist ein Fehler beim ausloggen passiert. Der User wurde nicht ausgeloggt.");
        }
        
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    
    public static async Task<IdentityUser> GetAspNetUserByEmail(UserManager<IdentityUser> userManager, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new Exception("AspNetUser nicht gefunden.");
        }

        return user;
    }
    
    public static async Task LoginUser(SignInManager<IdentityUser> signInManager, IdentityUser user, string password)
    {
        var result = await signInManager.PasswordSignInAsync(
            user.UserName ?? throw new Exception("user.UserName ist null."),
            password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new Exception("Invalid Credentials.");
        }
    }
}