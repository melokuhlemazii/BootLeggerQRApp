using System.ComponentModel.DataAnnotations;

namespace BootLeggerQRApp.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; } //primary key
        public int OrderId { get; set; }
        public int Rating { get; set; } //1–5 stars
        public string Comment { get; set; }
        public DateTime SubmittedAt { get; set; }

        //navigation
        public Order Order { get; set; }
    }
}
