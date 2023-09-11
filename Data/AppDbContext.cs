namespace ApiCamisetas.Data
{
    using ApiCamisetas.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Camiseta>? Camisetas { get; set; }
    }

}
