using System.ComponentModel.DataAnnotations;

namespace BootLeggerQRApp.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; } //PK
        public string TableNumber { get; set; }
        public string? QRCodeURL { get; set; }


        //navigation
        public ICollection<Order> Orders { get; set; }
    }
}
