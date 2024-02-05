using System.Text;
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
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("MySuperSecretKey"));

        services.AddGraphQLServer()
            .AddAuthorization()
            .AddType<VeranstalterType.VeranstalterType>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();
        
        
        // services.AddAuthentication(options =>
        // {
        //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        // }).AddJwtBearer(o =>
        // {
        //     o.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidIssuer = _configuration["Jwt:Issuer"],
        //         ValidAudience = _configuration["Jwt:Audience"],
        //         IssuerSigningKey = new SymmetricSecurityKey
        //             (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = false,
        //         ValidateIssuerSigningKey = true
        //     };
        // });
        //
        
        
        ////////
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(jwtBearerOptions =>
             {
                 jwtBearerOptions.Authority = _configuration["Authentication:Authority"];
                 jwtBearerOptions.Audience = _configuration["Authentication:Audience"];

                 jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
                 jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
                 jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
                 
                 
                 // jwtBearerOptions.TokenValidationParameters =
                 //     new TokenValidationParameters
                 //     {
                 //         ValidIssuer = "https://auth.chillicream.com",
                 //         ValidAudience = "https://graphql.chillicream.com",
                 //         ValidateIssuerSigningKey = true,
                 //         IssuerSigningKey = signingKey
                 //     };
             });
        ///////
            
        
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
        app.UseAuthorization();
        
        // app.UseWebSockets();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });
    }
}