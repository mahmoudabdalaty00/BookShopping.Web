using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User_Id { get; set; }

        public DateTime createdate { get; set; } = DateTime.Now;

        [Required]
        public int OrderStatus_Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<OrderDetails> OrderDetails { get; set; }



        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? Email { get; set; }
        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }
        [Required]
        [MaxLength(30)]
        public string? PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
 

    }
}
