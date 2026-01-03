using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootLeggerQRApp.Models
{
    public enum OrderStatus
    {
        Received,
        Preparing,
        Ready,
        Served
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; } //pk
        public int TableId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }

        [Column(TypeName = "Decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        //navigation
        public Table Table { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; }

    }
}
