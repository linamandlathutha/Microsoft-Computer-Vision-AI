using Microsoft.EntityFrameworkCore;

namespace WebApplication23.Models
{
    public class PhotoDbContext : DbContext
    {
        public PhotoDbContext(DbContextOptions<PhotoDbContext> options) : base(options) { }

        public DbSet<Photo> Photos { get; set; }
    }

}
