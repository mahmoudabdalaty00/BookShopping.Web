using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Repository
{
    public class UserOrderRepository : IUserOrderRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext context,
            IHttpContextAccessor contextAccessor
            , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel model)
        {
            var order = await _context.Order.FindAsync(model.OrderId);
            if (order == null) {
                throw new InvalidOperationException("Invailad data");
            }
        order.OrderStatus_Id = model.OrderStatusId;
            await _context.SaveChangesAsync(); 
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Order.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _context.OrderStatuses.ToListAsync();
        }
        public async Task<IEnumerable<OrderStatus>> GetOrderStatus()
        {
            throw new NotImplementedException();
        }


        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _context.Order.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Invailad data");
            }
            order.IsPaid = !order.IsPaid;
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Order>> UserOrder(bool getAll)
        {
            var order = _context.Order
                .Include(a => a.OrderStatus)
                .Include(b => b.OrderDetails)
                .ThenInclude(c => c.Book)
                .ThenInclude(d => d.Gendre)
                .AsQueryable();


            if (!getAll)
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                   
                }
                order = order.Where(a=> a.User_Id == userId);
                return await order.ToListAsync();
            }
            return await order.ToListAsync();

        }
        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
