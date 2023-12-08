using ApiHoaDon.Helper;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IAccountServices
    {
        ErorType RegisterAccount(RegisterModel registerModel);
        LoginResponse LoginAccount(LoginModel loginModel);
        ErorType LogoutAccount(int userId);
        void SendEmail(string email, string emailbody,string token);
        void SendResetPasswordEmail(string email,string token);
    }
}
