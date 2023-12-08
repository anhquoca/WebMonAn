namespace WebMonAn.Entities
{
    public class Product_type
    {
        public int Id { get; set; }
        public string Name_product_type { get; set; }
        public string? Image_type_product { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
