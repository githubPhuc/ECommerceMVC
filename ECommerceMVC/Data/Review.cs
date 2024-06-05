using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Data
{
    public partial class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public int HangHoaId { get; set; }
        public virtual HangHoa HangHoa { get; set; }
    }
}
