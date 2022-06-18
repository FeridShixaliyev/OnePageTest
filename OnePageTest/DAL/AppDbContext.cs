using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnePageTest.Models;

namespace OnePageTest.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Carts> Carts { get; set; }
    }
}
