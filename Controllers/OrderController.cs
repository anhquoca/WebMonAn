using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IOrderServices OrderSV;
        private readonly IVnPayService _vnPayService;
        public readonly AppDbContext DbContext;
        public OrderController(IVnPayService vnPayService)
        {
            DbContext = new AppDbContext();
            OrderSV = new OrderServices();
            _vnPayService = vnPayService;
        }
        [HttpPost("ThanhToan")]
        public IActionResult Order(OrderModel order) {
            if(order.PaymentId == 1)
            {
                var res = OrderSV.ThanhToan(order);
                return Ok(res);
            }
            if(order.PaymentId == 2)
            {
                var orderId = OrderSV.ThanhToanOnline(order);
                PaymentInformationModel model = new PaymentInformationModel {
                    Amount = order.Actual_price,
                    Name= "THANH TOAN DON HANG 0001",
                    OrderDescription="Mon An Ngon",
                    OrderType="VNPAY"
                };
                var urlketqua = _vnPayService.CreatePaymentUrl(model, HttpContext,orderId);
                return Ok(new
                {
                    Urlketqua = urlketqua
                });
            }
            return Ok("Hay chon hinh thuc thanh toan hop le!!");

        }
        [HttpGet("paymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if(response.Success == true)
            {
                var orderid = int.Parse(response.OrderId);
                OrderSV.XuLyDonDatHangThanhToanOnline(orderid);
            }

            return Ok(response);
        }
        [HttpPost("XuLyDonDathang")]
        public IActionResult Xulydondathang(OrderProcessingModel model)
        {
            var res = OrderSV.XuLyDonDatHang(model);
            return Ok(res);
        }
        [HttpPost("ListOrderDetail")]
        public IActionResult Orderdeltai(int UserId)
        {
            var res = OrderSV.GetOrder_deltail(UserId);
            return Ok(res);
        }
        [HttpPost("ListOrder")]
        public IActionResult ListOrder(int UserId)
        {
            var res = OrderSV.GetOrders(UserId);
            return Ok(res);
        }
    }
}
