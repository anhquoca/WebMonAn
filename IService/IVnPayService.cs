using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context , string orderId);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
