namespace WebMonAn.Entities
{
    public class Order_status
    {
        public int Id { get; set; }
        public string Status_name { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
    }
}
