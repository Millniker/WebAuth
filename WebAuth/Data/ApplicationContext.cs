using Microsoft.EntityFrameworkCore;
using Pathnostics.Web.Models.Entity;

namespace WebAuth.Data;

public class ApplicationContext:DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    public DbSet<UserEntity> User { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
            
    }
}
