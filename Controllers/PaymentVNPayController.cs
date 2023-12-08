using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;
using Microsoft.Extensions.Configuration;
namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentVNPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        public PaymentVNPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpPost("createPaymentUrl")]
        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var urlketqua = _vnPayService.CreatePaymentUrl(model, HttpContext,"9992345");

            return Ok( new
            {
                urlketqua = urlketqua
            });
        }
        [HttpGet("paymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Ok(response);
        }
    }
}
