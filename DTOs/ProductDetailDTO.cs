namespace WebMonAn.DTOs
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public int Product_typeId { get; set; }
        public ProductTypeDTO? Product_type { get; set; }
        public string Name_product { get; set; }
        public Double Price { get; set; }
        public string Avartar_image_product { get; set; }
        public int? Discount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Number_of_views { get; set; }
        public Double? Point_evaluation { get; set; }
        public int? Number_of_solds { get; set; }
    }
}
