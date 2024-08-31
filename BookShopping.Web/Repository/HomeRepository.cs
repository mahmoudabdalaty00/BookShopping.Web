using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Repositry
{
    public class HomeRepository : IHomeRepository
    {

        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Gendre>> GetGenre()
        {
            return _context.Gendres.ToList();
        }
        public async Task<IEnumerable<Book>> GetBook(string sterm = "", int gendreId = 0)
        {
            sterm = sterm.ToLower();

            IEnumerable<Book> books =await (
                         from book in _context.Books
                         join gendre in _context.Gendres
                         on book.GendreId equals gendre.Id
                         join stock in _context.Stocks
                         on book.Id equals stock.BookId
                         into book_stocks
                         from bookWithStock in book_stocks.DefaultIfEmpty()
                         where string.IsNullOrWhiteSpace(sterm) ||
                       (book.Title != null && book.Title.ToLower().StartsWith(sterm))

                         select new Book
                         {
                             Id = book.Id,
                             Title = book.Title,
                             GendreId = book.GendreId,
                             Author = book.Author,
                             Image = book.Image,
                             Price = book.Price,
                             GenreName = gendre.GendreName,
                             Quantity = bookWithStock == null ? 0 : bookWithStock.Quantity,

                         }
           ).ToListAsync();

            if (gendreId > 0)
            {
                books = books.
                    Where(a => a.GendreId == gendreId).ToList();
            }

            return books;
        }
    }
}
