using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("Gendre")]
    public class Gendre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? GendreName { get; set; }

        public List<Book> Books { get; set; }

    }
}
