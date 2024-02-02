using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.Schema.Queries;
using Ticketing2.GraphQL.Web.Services;
using Ticketing2.GraphQL.Web.Services.Veranstalter;

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
        services.AddGraphQLServer()
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