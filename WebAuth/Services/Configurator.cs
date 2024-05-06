using Microsoft.EntityFrameworkCore;
using WebAuth.Data;

namespace Pathnostics.Web.Services;

public abstract class Configurator
{
    public static void Migrate(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();
        }
    }
}