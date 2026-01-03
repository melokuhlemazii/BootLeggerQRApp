using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootLeggerQRApp.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        [Column(TypeName = "Decimal(18,2)")]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public string ImagePath { get; set; }


        //navigation
        public ICollection<OrderItem> OrderItems { get; set; }


    }
}
