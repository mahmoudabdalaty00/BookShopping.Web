using System.ComponentModel.DataAnnotations;

namespace BookShopping.Web.Models.DTOS
{
    public class CheckoutModel
    {
        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength (50)]
        public string? EmailAddress { get; set; }


        [Required]
        [MaxLength (20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }  

    }
}
