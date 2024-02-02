using Microsoft.EntityFrameworkCore;
using Ticketing.GraphQL.Web.DataLoaders;
using Ticketing.GraphQL.Web.Schema.Mutations;
using Ticketing.GraphQL.Web.Schema.Queries;
using Ticketing.GraphQL.Web.Schema.Subscriptions;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.Services.Courses;
using Ticketing.GraphQL.Web.Services.Instructors;

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
        services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddInMemorySubscriptions();
        
        var connectionString = _configuration.GetConnectionString("default");
        services.AddPooledDbContextFactory<SchoolDbContext>(
            o => o.UseSqlite(connectionString).LogTo(Console.WriteLine)
        );

        services.AddScoped<CoursesRepository>();
        services.AddScoped<InstructorsRepository>();
        services.AddScoped<InstructorDataLoader>();
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