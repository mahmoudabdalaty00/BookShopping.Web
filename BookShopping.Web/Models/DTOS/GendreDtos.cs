using System.ComponentModel.DataAnnotations;

namespace BookShopping.Web.Models.DTOS
{
    public class GendreDtos
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? GendreName { get; set; }
    }
}
