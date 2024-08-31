using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Controllers
{

    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            try
            {
                var cartCount = await _cartRepo.AddItem(bookId, qty);
                if (redirect == 0)
                    return Ok(cartCount);
                return RedirectToAction("GetUserCart");
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null)
                {
                    // Log or handle the specific SQL error
                    Console.WriteLine(sqlException.Message);
                }
                else
                {
                    // Handle other types of exceptions
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("GetUserCart");

        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepo.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {

            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepo.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
