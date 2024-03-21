using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Repository
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GettAllBooks();
        Task<BookModel> GetBookDetailsById(int BookId);
        Task<int> AddBook(BookModel bookModel);
        Task<int> UpdateBookById(int id, BookModel bookModel);
        Task<int> UpdateBookByIdPatch([FromRoute] int id, [FromBody] JsonPatchDocument bookModel);
        Task<int> DeleteBookById(int id);
    }
}
