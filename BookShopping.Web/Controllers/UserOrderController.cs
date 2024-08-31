using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookShopping.Web.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userorder;

        public UserOrderController(IUserOrderRepository userorder)
        {
            _userorder = userorder;
        }

        public async Task<IActionResult> UserOrders( )
        {
            var order = await _userorder.UserOrder();

            return View(order);
        }
    }
}
