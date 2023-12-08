using ApiHoaDon.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;
using static System.Net.WebRequestMethods;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAccountServices AccountSV;
        public readonly AppDbContext DbContext;
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            AccountSV = new AccountServices();
            DbContext = new AppDbContext();
            _configuration = configuration;
        }
        [HttpPost("register")]
        public IActionResult Dangky( RegisterModel model)
        {
            var res = AccountSV.RegisterAccount(model);
            var user = DbContext.User.FirstOrDefault(x=>x.Email == model.Email);
            if (user != null)
            {
                string email = model.Email;
                string emailbody = "xin chao ban ";
                string token = GenerateResetToken();
                user.VerifyEmailToken = token;
                DbContext.User.Update(user);
                DbContext.SaveChanges();

                AccountSV.SendEmail(email, emailbody, token);
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
           
        }
        [HttpPost("VerifyEmail")]
        public IActionResult VerifyEmail(VerifyEmailModel model)
        {
            // Verify the email using the token
            var user = DbContext.User.FirstOrDefault(x=>x.VerifyEmailToken == model.Token && x.Email == model.Email);

            if (user!=null)
            {
                var account = DbContext.Account.FirstOrDefault(x=>x.UserId == user.Id);
                if (account!=null)
                {
                    account.Verify = "Da xac minh tai khoan";
                    DbContext.Update(account);
                    DbContext.SaveChanges() ;
                    return Ok(ErorType.ThanhCong);
                }
                else
                {
                    return Ok(ErorType.ThatBai);
                }

            }
            else
            {
                return Ok(ErorType.ThanhCong);
            }
        }
        [HttpPost("login")]
        public IActionResult Login( LoginModel model)
        {
            var user = DbContext.User.FirstOrDefault(x=>x.Email == model.Email);
            if (user == null)
            {
                return Ok("Email khong ton tai!");
            }
            var account = DbContext.Account.FirstOrDefault(x => x.UserId == user.Id);
            string passwordAccount = account.Password;
            
            if (passwordAccount != model.Password)
            {
                return Ok("Sai password!");
            }
            string token = CreateToken(user);
            string resetPasswordToken = GenerateResetPasswordToken();
            DateTime resetPasswordTokenExpiry = DateTime.Now.AddDays(1);
            //SetRefreshToken(refreshToken);
            // save token
            account.Token = token;
            account.ResetPasswordToken =resetPasswordToken;
            account.ResetPasswordTokenExpiry = resetPasswordTokenExpiry;
            DbContext.Update(account);
            DbContext.SaveChanges();

            var result = new LoginResponse
            {
                UserId= user.Id,
                Token = token,
                ResetPasswordToken = resetPasswordToken,
                ResetPasswordTokenExpiry = resetPasswordTokenExpiry
            };
            return Ok(result);
        }
        [HttpPost("logout/{userid}")]
        public IActionResult Logout(int userid)
        {
            var res = AccountSV.LogoutAccount(userid);
            return Ok(res);
        }


        [HttpPost("ForgotPassword/{email}")]
        public IActionResult ForgotPassword(string email)
        {
            var user = DbContext.User.FirstOrDefault(x=>x.Email == email);
            
            if (user != null)
            {
                var account = DbContext.Account.FirstOrDefault(x => x.UserId == user.Id);
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenjwt = new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenjwt);
                AccountSV.SendResetPasswordEmail(email, token);
                account.Token=token;
                DbContext.Update(account);
                DbContext.SaveChanges();
                return Ok("Đã gửi emamil");
            }
            else
            {
                return Ok("Email khong tồn tại");
            }
        }
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            var account = DbContext.Account.FirstOrDefault(x => x.Token == model.Token);
            if(account != null)
            {
                if (account.Password != model.Password)
                {
                    account.Password = model.Password;
                    DbContext.Update(account);
                    DbContext.SaveChanges();
                    return Ok(ErorType.ThanhCong);
                }
                else
                {
                    return Ok(ErorType.TrungVoiMatKhauCu);
                }
            }
            else
            {
                return Ok(ErorType.ThatBai);
            }
            
        }

        [HttpPost("RenewToken")]
        public IActionResult RenewToken([FromBody] LoginResponse model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                ClockSkew = TimeSpan.Zero,
                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: Token valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.Token, tokenValidateParam, out var validatedToken);
                //check 2: check thuat toan
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var ktHacMac = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!ktHacMac)//false
                    {
                        return BadRequest("sai thuat toan ma hoa");
                    }
                }
                //check 3: Check resetPasswordtoken exist in DB
                var account = DbContext.Account.FirstOrDefault(x => x.ResetPasswordToken == model.ResetPasswordToken);
                if (account == null)
                {
                    return BadRequest("ResetPasswordToken khong ton tai");
                }
                //check 4: check token
                if (account.Token != model.Token)
                {
                    return BadRequest("Token khong ton tai");
                }
                //create new token
                var user = DbContext.User.FirstOrDefault(x => x.Id == account.UserId);
                string token = CreateToken(user);
                string resetPasswordToken = GenerateResetPasswordToken();
                DateTime resetPasswordTokenExpiry = DateTime.Now.AddDays(1);
                //SetRefreshToken(refreshToken);
                // save token
                account.Token = token;
                account.ResetPasswordToken = resetPasswordToken;
                account.ResetPasswordTokenExpiry = resetPasswordTokenExpiry;
                DbContext.Update(account);
                DbContext.SaveChanges();

                var result = new LoginResponse
                {
                    Token = token,
                    ResetPasswordToken = resetPasswordToken,
                    ResetPasswordTokenExpiry = resetPasswordTokenExpiry
                };
                return Ok(result);

            }
            catch(Exception ex) {
                return BadRequest("model requied");
            }

        }

        [HttpGet("GetListUser")]
        [Authorize]
        public IActionResult Getlistuser()
        {
            if (User.IsInRole("Admin"))
            {
                var listuser = DbContext.User.ToList();
                return Ok(listuser);
            }
            else
            {
                return Ok(" nguoi dung kgong co quyen");
            }

        }

        private string CreateToken(User user)
        {
            var account = DbContext.Account.FirstOrDefault(x=>x.UserId == user.Id);
            var vaitro = DbContext.Decentralization.FirstOrDefault(x => x.Id == account.DecentralizationId);
            string role = vaitro.Authority_name;
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        private string GenerateResetPasswordToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        public static string GenerateResetToken()
        {
            byte[] tokenData = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenData);
            }
            return Convert.ToBase64String(tokenData);
        }
    }
}
