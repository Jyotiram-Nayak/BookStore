using System.ComponentModel.DataAnnotations;
namespace BookStore.Model
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
