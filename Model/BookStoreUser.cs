using Microsoft.AspNetCore.Identity;

namespace BookStore.Model
{
    public class BookStoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
