using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Client.Models;

namespace MyWebTemplateV2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
