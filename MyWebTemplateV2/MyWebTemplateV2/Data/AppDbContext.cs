using Microsoft.EntityFrameworkCore;

namespace MyWebTemplateV2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add your DbSets here, for example:
        // public DbSet<YourModel> YourModels { get; set; }
    }
}
