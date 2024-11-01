using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    /// <summary>
    /// Context class for Db migration and creation
    /// </summary>
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
    }
}
