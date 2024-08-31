using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StockRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _context = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }




        public async Task<Stock?> GetStockByBookId(
            int bookId) => await _context.Stocks.FirstOrDefaultAsync(s => s.BookId == bookId
            );

        public async Task ManageStock(StockDTO stockToManage)
        {
            
            var existingStock = await GetStockByBookId(stockToManage.BookId);

            if (existingStock is null)
            {
                var stock = new Stock { BookId = stockToManage.BookId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from book in _context.Books
                                join stock in _context.Stocks
                                on book.Id equals stock.BookId
                                into book_stock
                                from bookStock in book_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || book.Title.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    BookId = book.Id,
                                    BookName = book.Title,
                                    Quantity = bookStock == null ? 0 : bookStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

    }
}
 
