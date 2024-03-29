using BookStore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class BookStoreDBContext : IdentityDbContext<BookStoreUser>
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> option) : base(option)
        {

        }
        public DbSet<Books> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() {Id="31168E7F-74BA-488E-8414-3AFBE6C2AAA3", Name = "Admin",NormalizedName = "ADMIN" },
                new IdentityRole() {Id= "0607928C-7CAA-4723-9C6B-9ED2449A2B2E", Name = "User",NormalizedName = "USER" }
                );
        }
    }
}
