using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootLeggerQRApp.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }//primary key
        public int OrderId { get; set; }
        public string Method { get; set; }//e.g., Card, Wallet

        [Column(TypeName = "Decimal(18,2)")]
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }

        //navigation
        public Order Order { get; set; }
    }
}
