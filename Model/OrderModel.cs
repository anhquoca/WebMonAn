using WebMonAn.Entities;

namespace WebMonAn.Model
{
    public class OrderModel
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public Double Original_price { get; set; }
        public Double Actual_price { get; set; }
        public string Full_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
    }
}
