using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Client.Models;

namespace MyWebTemplateV2.Data
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<AiKnowledge> AiKnowledge { get; set; }
    }
}
