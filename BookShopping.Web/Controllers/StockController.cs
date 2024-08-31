using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopping.Web.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stock1;



        public StockController(IStockRepository stock1)
        {
            _stock1 = stock1;
        }




        public async Task<IActionResult> Index(string sTerm = "")
        {
            var stock = await _stock1.GetStocks(sTerm);
            return View(stock);
        }




        public async Task<IActionResult> ManangeStock(int bookId)
        {
            var existingStock = await _stock1.GetStockByBookId(bookId);
            var stock = new StockDTO
            {
                BookId = bookId,
                Quantity = existingStock != null
            ? existingStock.Quantity : 0
            };
            return View(stock);
        }




        [HttpPost]
        public async Task<IActionResult> ManangeStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return View(stock);
            try
            {
                await _stock1.ManageStock(stock);
                TempData["successMessage"] = "Stock is updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something went wrong!!";
            }

            return RedirectToAction(nameof(Index));
        }







    }
}
