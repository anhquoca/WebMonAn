using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductServices ProductSV;
        public readonly AppDbContext DbContext;
        public ProductController()
        {
            DbContext = new AppDbContext();
            ProductSV = new ProductServices();
        }
        [HttpGet("GetProduct/{producId}")]
        public IActionResult getProduct(int producId)
        {
            var result = ProductSV.GetProduct(producId);
            return Ok(result);
        }
        [HttpGet("GetListProductCungLoaiVoiProduct/{producId}")]
        public IActionResult getListProductCungLoai(int producId)
        {
            var result = ProductSV.GetListProductTheoType(producId);
            return Ok(result);
        }
        [HttpGet("GetListProductTheoDanhSach")]
        public IActionResult getListProductTheoDanhSach()
        {
            var listProductType = DbContext.Product_type.OrderByDescending(x=>x.Products.Count()).Select(x=> new ProductTypeDTO
            {
                Id = x.Id,
                Name_product_type=x.Name_product_type
            }).ToList();
            return Ok(listProductType); 
        }
        [HttpGet("GetListProduct")]
        public IActionResult getListProduct()
        {
            var listProduct = ProductSV.GetListProducts();
            return Ok(listProduct);
        }
        [HttpGet("GetListProductBanChay")]
        public IActionResult getListProductBanChay()
        {
            var listProduct = ProductSV.GetProductListBanChay();
            return Ok(listProduct);
        }
        [HttpGet("GetListProductTheoType/{productTypeId}")]
        public IActionResult getListProductTheoType(int productTypeId)
        {
            var result =DbContext.Product.Select(x => new ProductDTO
            {
                Id = x.Id,
                Product_typeId = x.Product_typeId,
                Name_product = x.Name_product,
                Avartar_image_product = x.Avartar_image_product,
                Price = x.Price,
                Discount = x.Discount

            }).Where(x => x.Product_typeId == productTypeId).ToList();
            return Ok(result);
        }
    }
}
