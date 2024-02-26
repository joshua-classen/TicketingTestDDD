using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Ticketing.GraphQL.Web.GraphQLTypes;
using Ticketing.GraphQL.Web.Repository;
using Ticketing.GraphQL.Web.Schema.Mutations;
using Ticketing.GraphQL.Web.Schema.Queries;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.Stripe;

namespace Ticketing.GraphQL.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddGraphQLServer()
            .AddAuthorization()
            .AddType<VeranstaltungType>()
            .AddType<PurchasedTicketType>()
            .AddType<KundeUserType>()
            .AddType<VeranstalterUserType>()
            .AddQueryType(d => d.Name("Query"))
            .AddTypeExtension<QueryVeranstaltung>()
            .AddTypeExtension<QueryKundeUser>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddType<MutationKunde>()
            .AddType<MutationVeranstalter>()
            .AddType<MutationVeranstaltung>()
            .AddType<MutationPurchaseTicket>();
        
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<TicketingDbContext>()
            .AddDefaultTokenProviders();
        
        // Konfiguriere JWT-Authentifizierung
        var jwtIssuer = _configuration.GetSection("Jwt:Issuer").Get<string>();
        var jwtKey = _configuration.GetSection("Jwt:Key").Get<string>();
        if (jwtIssuer is null || jwtKey is null) // todo: überarbeiten.
        {
            throw new Exception("Jwt:Issuer or Jwt:Key not found in appsettings.json");
        }
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            };
        });
        
        StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("TicketingDDD_Stripe__StripeConfigurationApiKey")
                                     ?? SecretsGetterRepository.GetStripeSkTestSecret().Result;
        
        // Konfiguriere Autorisierung mit Rollen
        services.AddAuthorization(options =>
        {
            options.AddPolicy("VeranstalterPolicy", policy => policy.RequireRole("Veranstalter"));
            options.AddPolicy("KundenPolicy", policy => policy.RequireRole("Kunde"));
        });
        
        var connectionString = _configuration.GetConnectionString("default");
        services.AddPooledDbContextFactory<TicketingDbContext>(o => o.UseSqlite(connectionString));
        
        
        //ok hier noch überarbeiten. Das wird nur initialisiert wenn es auch in dem rest gebraucht wird.
        
        services.AddSingleton<IStripeEnpointWebHookSecret>(new StripeEndpointWebHookSecretResolver());
        services.AddScoped<UserManager<IdentityUser>>();
        services.AddScoped<SignInManager<IdentityUser>>();
        services.AddScoped<TicketingDbContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGraphQL();
        });
    }
}