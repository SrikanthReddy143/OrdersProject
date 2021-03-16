using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrdersProject.Models
{
    public class OrderedItems
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public int UserId { get; set; }
        public string CreatedDate { get; set; }
        public IEnumerable<ItemsList> itemsLists { get; set; }
    }
    public class ItemsList
    {
        public int ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public int Price { get; set; }
    }
}