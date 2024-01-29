using Ticketing.GraphQL.Web.Schema;

namespace Ticketing.GraphQL.Web;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGraphQLServer().AddQueryType<Query>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });
    }
}