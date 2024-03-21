using System.ComponentModel.DataAnnotations;

namespace BookStore.Data
{
    public class Books
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
