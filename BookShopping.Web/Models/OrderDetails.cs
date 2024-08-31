using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("OrderDetails")]

    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int Quantity { get; set; }


        [Required]
        public double? UnitPrice { get; set; }


        [Required]
        public int Order_Id { get; set; }
        public Order Order { get; set; }

        [Required]
        public int Book_Id { get; set; }
        public Book Book { get; set; }

    }
}
