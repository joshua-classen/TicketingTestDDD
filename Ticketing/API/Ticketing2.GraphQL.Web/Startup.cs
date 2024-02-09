using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ticketing2.GraphQL.Web.Schema.Mutations;
using Ticketing2.GraphQL.Web.Schema.Queries;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        // var signingKey = new SymmetricSecurityKey(
        //     Encoding.UTF8.GetBytes("MySuperSecretKey"));

        services.AddGraphQLServer()
            .AddAuthorization()
            .AddType<VeranstalterType.VeranstalterType>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ticket.com",
                    ValidAudience = "Veranstalter",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOMyKEY!@#1234"))
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
            {
                policy.RequireRole("Admin");
            });
        });
            
        
        var connectionString = _configuration.GetConnectionString("default");
        services.AddPooledDbContextFactory<TicketingDbContext>(
            o => o.UseSqlite(connectionString)//.LogTo(Console.WriteLine)
        );

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
        // app.UseAuthorization();
        
        // app.UseWebSockets();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });
    }
}