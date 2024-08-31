namespace BookShopping.Web.Models.DTOS
{
    public class OrderDetailsDtoscs
    {
        public string DivId { get; set; }

        public IEnumerable<OrderDetails> Details { get; set; }
    }
}
