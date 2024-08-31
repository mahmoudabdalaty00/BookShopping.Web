using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]

        public string? Title { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

        [Required]
        public string? Author { get; set; }


        public int? GendreId { get; set; }

        public Gendre? Gendre { get; set; }

        public List<OrderDetails>? OrderDetails { get; set; }
        public List<CartDetails>? CartDetails { get;set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string GenreName { get; set; }
        [NotMapped]

        public int Quantity { get; set; }


    }
}
