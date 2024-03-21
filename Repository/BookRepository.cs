using AutoMapper;
using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class BookRepository:IBookRepository
    {
        private readonly BookStoreDBContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<BookModel>> GettAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(books);

        }
        public async Task<BookModel> GetBookDetailsById(int BookId)
        {
            //var book = await _context.Books.Where(x => x.Id == BookId).Select(x => new BookModel()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).FirstOrDefaultAsync();
            //return book;
            var book = await _context.Books.FindAsync(BookId);
            return _mapper.Map<BookModel>(book);
        }
        public async Task<int> AddBook(BookModel bookModel)
        {
            var book=new Books()
            {
                Title= bookModel.Title,
                Description=bookModel.Description
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task<int> UpdateBookById(int id,BookModel bookModel)
        {
            var book = await _context.Books.FindAsync(id);
            if(book!=null)
            {
                book.Title = bookModel.Title;
                book.Description = bookModel.Description;
            };
            var status = await _context.SaveChangesAsync();
            return status;
        }
        public async Task<int> DeleteBookById(int id)
        {
            var book =_context.Books.Where(X => X.Id == id).FirstOrDefault();
            if(book != null)
            {
                _context.Books.Remove(book);
                var status = await _context.SaveChangesAsync();
                return status; 
            }
            return 0;
        }
        public async Task<int> UpdateBookByIdPatch([FromRoute] int id, [FromBody] JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(id);
            if(book!=null)
            {
                bookModel.ApplyTo(book);
                var status = await _context.SaveChangesAsync();
                return status;
            }
            return 0;
        }
    }
}
