using WebMonAn.Entities;

namespace WebMonAn.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int Product_typeId { get; set; }
        public string Name_product { get; set; }
        public Double Price { get; set; }
        public string Avartar_image_product { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Discount { get; set; }
    }
}
