using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly IUserServices UserSV;
        public readonly IAdminServices AdminSV;
        public readonly IOrderServices OrderSV;
        public readonly AppDbContext DbContext;
        public AdminController()
        {
            DbContext = new AppDbContext();
            UserSV = new UserServices();
            AdminSV = new AdminServices();
            OrderSV = new OrderServices();
        }
        [HttpGet("GetOrders")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetOrders()
        {
            var listResult = AdminSV.GetOrders();
            return Ok(listResult);
        }
        [HttpGet("GetOrderDetail/{orderId}")]
        public IActionResult GetOrderDetail(int orderId)
        {
            var listResult = AdminSV.GetOrder_deltail(orderId);
            return Ok(listResult);
        }
        [HttpPost("Thongkedoanhthu"), Authorize(Roles = "Admin")]
        public IActionResult Thongke(ThongKeModel model)
        {
            var res = UserSV.ThongKeDoanhThu(model.month, model.quarter, model.year);
            return Ok(res);
        }
        [HttpGet("GetYearOrders")]
        public IActionResult GetYear()
        {
            var listYear = DbContext.Order.Select(x => x.Created_at.Value.Year).Distinct().ToList();
            return Ok(listYear);
        }
        [HttpGet("GetListProductType")]
        public IActionResult Getlist()
        {
            var listResult = AdminSV.GetProductTypes();
            return Ok(listResult);
        }
        [HttpPost("ThemProduct")]
        public IActionResult Themsanpham(ProductModel product)
        {
            var res = AdminSV.ThemProduct(product);
            return Ok(res);
        }
        [HttpPost("ThemProductType")]
        public IActionResult ThemProductType(ProductTypeModel productType)
        {
            var res = AdminSV.ThemProductType(productType);
            return Ok(res);
        }
        [HttpPut("SuaProduct")]
        public IActionResult UpdateProductType(int productTypeId, ProductTypeModel productType)
        {
            var res = AdminSV.UpdateProductType(productTypeId, productType);
            return Ok(res);
        }
        [HttpDelete("XoaProduct")]
        public IActionResult DeleteProductType(int productTypeId)
        {
            var res = AdminSV.DeleteProductType(productTypeId);
            return Ok(res);
        }
    }
}
