namespace WebMonAn.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string Payment_method { get; set; }
        public int? Status { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
