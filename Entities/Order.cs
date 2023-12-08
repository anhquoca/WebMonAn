namespace WebMonAn.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public Double Original_price {  get; set; }
        public Double Actual_price { get; set; }
        public string Full_Name {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Note {  get; set; }
        public int Order_statusId { get; set; }
        public Order_status? Order_status { get; set; } 
        public IEnumerable<Order_detail>? Order_Details { get; set;}
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
