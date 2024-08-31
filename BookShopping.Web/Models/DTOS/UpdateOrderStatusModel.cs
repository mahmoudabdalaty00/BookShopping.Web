using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookShopping.Web.Models.DTOS
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        public IEnumerable<SelectListItem> selectListItems { get; set; }
    }
}
