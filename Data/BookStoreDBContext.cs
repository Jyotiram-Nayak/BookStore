using BookStore.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class BookStoreDBContext:IdentityDbContext<BookStoreUser>
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> option):base(option)
        {
                
        }
        public DbSet<Books> Books { get; set; }
    }
}
