namespace WebMonAn.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Double Original_price { get; set; }
        public Double Actual_price { get; set; }
        public string Full_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Payment_method { get; set; }
        public string Status_name { get; set; }
        public string Created_at { get; set; }
    }
}
