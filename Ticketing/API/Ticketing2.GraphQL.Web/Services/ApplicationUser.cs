using Microsoft.AspNetCore.Identity;

namespace Ticketing2.GraphQL.Web.Services;

public class ApplicationUser : IdentityUser<Guid>
{
    
    // todo: überarbeiten
    public ApplicationUser()
    {
    }
    
    public ApplicationUser(string userName) : base(userName)
    {
    }
}


/*

Ich habe folgenden Code
public class ApplicationRole : IdentityRole<Guid>
   {
       
   }


public class ApplicationUser : IdentityUser<Guid>
   {
       
   }
   
   public class TicketingDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
   {
       public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
       {
       }
   
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingDbContext).Assembly); // Was macht das hier? 
       }
       
       public DbSet<KundeUser> KundeUser { get; set; }
       public DbSet<VeranstalterUser> VeranstalterUser { get; set; }
       public DbSet<Veranstaltung> Veranstaltung { get; set; } // sollte man den Namen hier in Singular oder Plural schreiben?
       public DbSet<PurchasedTicket> PurchasedTickets { get; set; } // ich glaube das muss hier rein
   }
   
   
   Ich erstelle die migrations mit dotnet ef migrations add Initial
   
   
   Dann erhalte ich folgenden Fehler im Terminal:
   Build started...
   Build succeeded.
   The Entity Framework tools version '8.0.0' is older than that of the runtime '8.0.1'. Update the tools for the latest features and bug fixes. See https://aka.ms/AAc1fbw for more information.
   An error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: Microsoft.AspNetCore.Identity.UserManager`1[Microsoft.AspNetCore.Identity.IdentityUser] Lifetime: Scoped ImplementationType: Microsoft.AspNetCore.Identity.UserManager`1[Microsoft.AspNetCore.Identity.IdentityUser]': Unable to resolve service for type 'Microsoft.AspNetCore.Identity.IUserStore`1[Microsoft.AspNetCore.Identity.IdentityUser]' while attempting to activate 'Microsoft.AspNetCore.Identity.UserManager`1[Microsoft.AspNetCore.Identity.IdentityUser]'.) (Error while validating the service descriptor 'ServiceType: Microsoft.AspNetCore.Identity.SignInManager`1[Microsoft.AspNetCore.Identity.IdentityUser] Lifetime: Scoped ImplementationType: Microsoft.AspNetCore.Identity.SignInManager`1[Microsoft.AspNetCore.Identity.IdentityUser]': Unable to resolve service for type 'Microsoft.AspNetCore.Identity.IUserStore`1[Microsoft.AspNetCore.Identity.IdentityUser]' while attempting to activate 'Microsoft.AspNetCore.Identity.UserManager`1[Microsoft.AspNetCore.Identity.IdentityUser]'.)
   Unable to create a 'DbContext' of type ''. The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[Ticketing2.GraphQL.Web.Services.TicketingDbContext]' while attempting to activate 'Ticketing2.GraphQL.Web.Services.TicketingDbContext'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
   




*/