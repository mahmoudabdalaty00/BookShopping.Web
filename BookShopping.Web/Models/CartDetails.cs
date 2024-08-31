using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("CartDetails")]
    public class CartDetails
    {
        [Key]
         public int Id { get; set; }



        [Required]
        public int ShoppingCart_Id { get; set; }

        public ShoppingCart? ShoppingCart { get; set; }  



         [Required]
        public int Book_Id { get; set; }

        public Book? Books { get; set; }  

        public int Quantity { get; set; }   

        public double UnitPrice { get; set; }

        public int? StockQuantity { get;   set; }

    }
}
