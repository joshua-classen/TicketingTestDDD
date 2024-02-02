using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        using (IServiceScope scope = host.Services.CreateScope())
        {
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TicketingDbContext>>();
            
            using(TicketingDbContext context = contextFactory.CreateDbContext())
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
