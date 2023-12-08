namespace WebMonAn.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int Product_typeId { get; set;}
        public Product_type? Product_type { get; set;}
        public string Name_product { get; set;}
        public Double Price {  get; set;}   
        public string Avartar_image_product { get; set;}
        public string Title {  get; set;}  
        public string Description { get; set;}
        public int? Discount { get; set;}
        public int? Status { get; set; }
        public int? Number_of_views { get; set; }
        public int? Number_of_solds { get; set; }
        public IEnumerable<Cart_item>? Cart_items { get; set;}
        public IEnumerable<Order_detail>? Order_details { get; set;}
        public IEnumerable<Product_review>? Product_reviews { get; set;}
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
