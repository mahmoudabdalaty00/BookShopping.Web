﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Web.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StatusId  { get; set; }


        [Required]
        [MaxLength(50)]
        public string? StatusName { get; set; }
    }
}
