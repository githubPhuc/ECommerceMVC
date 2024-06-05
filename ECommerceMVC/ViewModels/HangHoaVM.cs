using ECommerceMVC.Data;

namespace ECommerceMVC.ViewModels
{
	public class HangHoaVM
	{
		public int MaHh { get; set; }
		public string TenHH { get; set; }
		public string Hinh { get; set; }
		public double DonGia { get; set; }
		public string MoTaNgan { get; set; }
		public string TenLoai { get; set; }
	}

	public class ChiTietHangHoaVM
	{
		public int MaHh { get; set; }
		public string TenHH { get; set; }
		public string Hinh { get; set; }
		public double DonGia { get; set; }
		public string MoTaNgan { get; set; }
		public string TenLoai { get; set; }
		public string ChiTiet { get; set; }
		public int DiemDanhGia { get; set; }
		public int SoLuongTon { get; set; }
        public List<ReviewVM> Reviews { get; set; } = new List<ReviewVM>();
    }

    public class ReviewVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }

}

