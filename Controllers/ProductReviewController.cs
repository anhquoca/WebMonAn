using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        public readonly IProductReviewServices ProductReviewSV;
        public readonly AppDbContext DbContext;
        public ProductReviewController()
        {
            DbContext = new AppDbContext();
            ProductReviewSV = new ProductReviewServices();
        }
        [HttpGet("getProductReviews/{productId}")]
        public IActionResult GetProductReview(int productId)
        {
            var res = ProductReviewSV.GetProductReviews(productId);
            return Ok(res);
        }
        [HttpPost("ThemDanhGiaSanPham")]
        public IActionResult DanhGiaSanPham([FromBody]ProductReviewModel model)
        {
            var res  = ProductReviewSV.UserAddtProductReviews(model);
            return Ok(res);
        }
        [HttpPost("KiemTraQuyenDanhGiaSanPham")]
        public IActionResult KiemTra(UserProductModel model)
        {
            var res = ProductReviewSV.KiemTraQuyenDanhGiaSanPham(model);
            return Ok(res);
        }
    }
}
