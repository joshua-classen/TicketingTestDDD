using Microsoft.EntityFrameworkCore;
using Ticketing.GraphQL.Web.Services;

namespace Ticketing.GraphQL.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        using (IServiceScope scope = host.Services.CreateScope())
        {
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
            
            using(SchoolDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }
        }
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
