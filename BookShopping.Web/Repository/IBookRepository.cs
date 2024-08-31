namespace BookShopping.Web.Repository
{
    public interface IBookRepository
    {
        Task AddBook(Book book) ;
        Task DeleteBook(Book book);
        Task EditBook(Book book) ;
        Task<Book> GetBookById(int id);

        Task<IEnumerable<Book>> GetAllBooks();
    }
}
