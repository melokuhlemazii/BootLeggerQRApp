using System.ComponentModel.DataAnnotations;

namespace BootLeggerQRApp.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; } //pk
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstructions { get; set; }

        //navigation
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }

    }

}

