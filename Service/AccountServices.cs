using ApiHoaDon.Helper;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class AccountServices : IAccountServices
    {
        public readonly AppDbContext DbContext;
        public AccountServices()
        {
            DbContext = new AppDbContext();
        }
        public LoginResponse LoginAccount(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public ErorType LogoutAccount(int userId)
        {
            var account = DbContext.Account.FirstOrDefault(x => x.UserId == userId);
            if(account != null)
            {
                account.Token = "";
                account.ResetPasswordToken = "";
                account.ResetPasswordTokenExpiry = null;
                DbContext.Update(account);
                DbContext.SaveChanges();
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.ThatBai;
            }
        }

        public ErorType RegisterAccount(RegisterModel model)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    if (DbContext.User.Any(x => x.Email == model.Email))
                    {
                        return ErorType.EmailDaTonTai;
                    }
                    var User = new User
                    {
                        Email = model.Email,
                    };
                    DbContext.User.Add(User);
                    DbContext.SaveChanges();
                    var userTemp = DbContext.User.FirstOrDefault(x => x.Email == model.Email);
                    var Account = new Account()
                    {
                        User_name = model.User_name,
                        Password = model.Account_password,
                        UserId = userTemp.Id,
                        DecentralizationId = 2
                    };
                    DbContext.Account.Add(Account);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return ErorType.ThanhCong;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    return ErorType.ThatBai;
                }
            }
            
            
        }

        public void SendEmail(string email, string emailBody, string token)
        {
            
                var sendEmail = new MimeMessage();
                sendEmail.From.Add(MailboxAddress.Parse("nguyenanhquoc3042612@gmail.com"));
                sendEmail.To.Add(MailboxAddress.Parse(email));
                sendEmail.Subject = "xin chao nguoi dung moi!";
                string body = $"đây là mã xác thực của ban: {token}" +
                $"Nhấn vào liên kết để xác thực tài khoản cua bạn : http://localhost:3000/AccountAuthentication?email={email}?token={token}";
                sendEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


            using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("nguyenanhquoc3042612@gmail.com", "jrodmstwjyahwgqv");
                smtp.Send(sendEmail);
                smtp.Disconnect(true);
        }

        public void SendResetPasswordEmail(string email, string token)
        {
            var sendEmail = new MimeMessage();
            sendEmail.From.Add(MailboxAddress.Parse("nguyenanhquoc3042612@gmail.com"));
            sendEmail.To.Add(MailboxAddress.Parse(email));
            sendEmail.Subject = "Đặt lại mật khẩu!";
            string body = $"Nhấn vào liên kết sau để đặt lại mật khẩu: http://localhost:3000/confirm-password/reset-password?token={token}";
            sendEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("nguyenanhquoc3042612@gmail.com", "jrodmstwjyahwgqv");
            smtp.Send(sendEmail);
            smtp.Disconnect(true);
        }
    }
}
