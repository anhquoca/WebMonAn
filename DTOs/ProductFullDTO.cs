using WebMonAn.Entities;

namespace WebMonAn.DTOs
{
    public class ProductFullDTO
    {
        public int Id { get; set; }
        public int Product_typeId { get; set; }
        public ProductTypeDTO? Product_type { get; set; }
        public string Name_product { get; set; }
        public Double Price { get; set; }
        public string Avartar_image_product { get; set; }
        public int? Discount { get; set; }
    }
}
