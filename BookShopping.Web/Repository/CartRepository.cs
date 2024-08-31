using BookShopping.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShopping.Web.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
          
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
 



                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };

                    _db.ShoppingCart.Add(cart);
  
                }
              //   _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                  .FirstOrDefault(a => a.ShoppingCart_Id == cart.Id && a.Book_Id == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = await _db.Books.FindAsync(bookId);

                    if (book == null)
                    {
                        throw new Exception("Invalid BooksId");
                    }

                    cartItem = new CartDetails
                    {
                      //  StockQuantity = book.Stock.Quantity,
                      Books = book,
                      ShoppingCart = cart,
                        Book_Id = bookId,
                        ShoppingCart_Id = cart.Id,
                        Quantity = qty,
                        UnitPrice = book.Price,
                      };
                    _db.CartDetails.Add(cartItem);
                    
                       
                    } 
                       
                      
                
              
              transaction.Commit();
                   _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<int> RemoveItem(int bookId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
              
                
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCart_Id == cart.Id && a.Book_Id == bookId);
                if (cartItem is null)
                    throw new InvalidOperationException("No items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.ShoppingCart
                                    .Include(a => a.CartDetails)
                                    .ThenInclude(a => a.Books)
                                    .ThenInclude(a => a.Stock)
                                    .Include(a => a.CartDetails)
                                    .ThenInclude(a => a.Books)
                                    .ThenInclude(a => a.Gendre)
                                    .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
          public async Task<int>? GetStock(int bookid)
        {
            var stock = await _db.Stocks.FirstOrDefaultAsync(x => x.BookId == bookid);
            return stock.Quantity;
        }

 
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCart
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCart_Id
                              where cart.UserId == userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var trans = _db.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");

                var cart = await GetCart(userId);
                if (cart != null)
                    throw new InvalidOperationException("Invalid Cart");



                var cartDetails = _db.CartDetails
             .Where(a => a.ShoppingCart_Id == cart.Id).ToList();

                if (cartDetails.Count == 0)
                    throw new InvalidOperationException("Cart is Empty");





                var pendingRecord = _db.OrderStatuses.FirstOrDefault(
                    s => s.StatusName == "Pending"
                    );

               if (pendingRecord is null)
                 throw new InvalidOperationException("Order status does not have Pending status");



                var order = new Order
                {

                    User_Id = userId,
                    createdate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.EmailAddress,
                    MobileNumber = model.PhoneNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatus_Id = pendingRecord.Id
                };



                _db.Order.Add(order);
                _db.SaveChanges();


                foreach (var it in cartDetails)
                {
                    var orderDetail = new OrderDetails
                    {
                        Book_Id = it.Book_Id,
                        Order_Id = order.Id,
                        Quantity = it.Quantity,
                        UnitPrice = it.UnitPrice

                    };
                    _db.OrderDetails.Add(orderDetail);
                    _db.SaveChanges();


                    var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.BookId == it.Book_Id);
                    if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }

                    if (it.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items(s) are available in the stock");
                    }
                    stock.Quantity -= it.Quantity;
                }
               
                _db.CartDetails.RemoveRange(cartDetails);
                _db.SaveChanges();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }



        }



        public async Task UpdateCartQuantity(int bookId, int qty)
        {
            if (qty < 1)
            {
                throw new ArgumentException("Quantity should be greater than 0", nameof(qty));
            }
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged-in");
            }
            
            var cart = _db.ShoppingCart
                        .Include(c => c.CartDetails)
                        .FirstOrDefault(a => a.UserId == userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart does not exist.");
            }

            var cartItem = cart.CartDetails.FirstOrDefault(item => item.Book_Id == bookId);
            if (cartItem == null)
            {
                throw new InvalidOperationException("Cart item can not be null");
            }

            cartItem.Quantity = qty;

            await _db.SaveChangesAsync();
        }


    }
}

