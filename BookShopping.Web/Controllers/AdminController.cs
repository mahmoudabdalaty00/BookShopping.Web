using BookShopping.Web.Constants;
using BookShopping.Web.Models;
using BookShopping.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookShopping.Web.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class AdminController : Controller
    {
       private readonly ApplicationDbContext _context; 
        private readonly IUserOrderRepository _userOrderRepository;

        public AdminController(ApplicationDbContext context, IUserOrderRepository userOrderRepository)
        {
            _context = context;
            _userOrderRepository = userOrderRepository;
        }

        public async  Task<IActionResult> AllOrders()
        {
         var orders =  await _userOrderRepository.UserOrder(true);
            return View(orders);
        } 


        public async  Task<IActionResult> UpdateTogglePaymentmethod(int OrderId)
        {
         var orders =  await _userOrderRepository.GetOrderById(OrderId);
            if (orders == null) 
            {
                throw new InvalidOperationException("Invailad data");

            }

            var orderstatlist = (await 
                _userOrderRepository.GetOrderStatus()
                ).Select(orderstatus => { 
                return new SelectListItem {
                    Value = orderstatus.Id.ToString()
                    , Text = orderstatus.StatusName,
                    Selected = orders.OrderStatus_Id == orderstatus.Id 
                };
        });
 
        var data = new UpdateOrderStatusModel
        {
            OrderId = OrderId,
            OrderStatusId = orders.OrderStatus_Id,
            selectListItems = orderstatlist
        };
        return View(data);
    } 
        public async  Task<IActionResult> TogglePaymentmethod(int OrderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(OrderId);
            }
            catch (Exception ex) { }
            return RedirectToAction(nameof(AllOrders));
        }




    }
}
