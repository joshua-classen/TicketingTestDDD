using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.Schema.Queries;
using Ticketing2.GraphQL.Web.Services;
using Ticketing2.GraphQL.Web.Services.Veranstalter;

namespace Ticketing2.GraphQL.Web;


//todo: mir fehlt noch ein VeranstalterMapping. Also wie es auf die Datenbank gemappt wird.

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddType<VeranstalterType.VeranstalterType>()
            .AddQueryType<QueryTicketing>();
            
        
        var connectionString = _configuration.GetConnectionString("default");
        services.AddPooledDbContextFactory<TicketingDbContext>(
            o => o.UseSqlite(connectionString)//.LogTo(Console.WriteLine)
        );

        services.AddScoped<VeranstalterRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseWebSockets();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });
    }
}