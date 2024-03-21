using AutoMapper;
using BookStore.Data;
using BookStore.Model;

namespace BookStore.Mapper
{
    public class BookStoreMapper : Profile
    {
        public BookStoreMapper()
        {
            CreateMap<Books, BookModel>();
        }
    }
}
