using BookStore.Model;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    //[Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GettAllBooks();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookDetails([FromRoute] int id)
        {
            var book = await _bookRepository.GetBookDetailsById(id);
            return Ok(book);
        }
        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookModel bookModel)
        {
            var id = await _bookRepository.AddBook(bookModel);
            return CreatedAtAction(nameof(GetBookDetails), new { id = id, controller = "Book" }, id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookById([FromRoute] int id, [FromBody] BookModel bookModel)
        {
            var status = await _bookRepository.UpdateBookById(id, bookModel);
            if (status != 0)
            {
                return Ok("success");
            }
            return Ok("Faild to update");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookByIdPatch([FromRoute] int id, [FromBody] JsonPatchDocument bookModel)
        {
            var status = await _bookRepository.UpdateBookByIdPatch(id, bookModel);
            if (status != 0)
            {
                return Ok("success");
            }
            return Ok("Faild to update");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            var status = await _bookRepository.DeleteBookById(id);
            if (status != 0)
            {
                return Ok("success");
            }
            return Ok("Faild to delete");
        }
    }
}
