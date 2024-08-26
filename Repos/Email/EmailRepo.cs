using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Interfaces.Email;
using College_managemnt_system.models;
using College_managemnt_system.models.EmailModel;
using College_managemnt_system.Repos.Utilities;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;


namespace College_managemnt_system.Repos.Email
{
    public class EmailRepo : IEmailRepo
    {
        private readonly CollegeDBContext _context;
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IBlackListTokensRepo _blackListTokensRepo;
        private readonly ITokensRepo _tokensRepo;
        private readonly UtilitiesRepo _utilitiesRepo;

        public EmailRepo(CollegeDBContext context, IOptions<EmailSettings> emailSettings, IConfiguration configuration, IPasswordService passwordService, IBlackListTokensRepo blackListTokensRepo, ITokensRepo tokensRepo,UtilitiesRepo utilitiesRepo)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
            _configuration = configuration;
            _passwordService = passwordService;
            _blackListTokensRepo = blackListTokensRepo;
            _tokensRepo = tokensRepo;
            _utilitiesRepo = utilitiesRepo;
        }

        public async Task<CustomResponse<bool>> SendVerificationEmail(int accountId)
        {

            Account account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.AccountId == accountId);

            if (account == null)
                return new CustomResponse<bool>(404, "Account not found");

            if (account.Verified)
                return new CustomResponse<bool>(409, "Account already verfied");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(account.Email, account.Email));
            emailMessage.Subject = "Email verfication";

            string VerficationToken = _tokensRepo.generateVerficationJWT(accountId, _configuration["Jwt:Key"]);


            string verficationEmail = GetVerifcaitonEmailTemplate($"https://FrontEndDemo.com/Security/Verify?token={VerficationToken}"); // the Client side (frontEnd) then sends the auth token via the Authorization Bearer

            var bodyBuilder = new BodyBuilder { HtmlBody = verficationEmail };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {

                    client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);

                    client.Authenticate(_emailSettings.Username, _emailSettings.Password);

                    await client.SendAsync(emailMessage);
                    client.Disconnect(true);
                }
                return new CustomResponse<bool>(201, "Verfication email sent successfully");
            }
            catch (Exception ex)
            {
                return new CustomResponse<bool>(500, ex.Message);
            }
        }


        public async Task<CustomResponse<bool>> VerifyAccount(int accountId)
        {
            Account account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.AccountId == accountId);
            if (account == null)
                return new CustomResponse<bool>(404, "Account not found");

            if (account.Verified)
                return new CustomResponse<bool>(404, "Account already verfied");
            account.Verified = true;

            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Email verfied successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong please try again later");
            }
        }

        public async Task<CustomResponse<bool>> SendPasswordChangeEmail(string email)
        {
            if (!_utilitiesRepo.IsValidEmail(email))
                return new CustomResponse<bool>(400, "Plesae enter a correct email");


            Account account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.Email == email);

            if (account == null)
                return new CustomResponse<bool>(404, "Account not found");


            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(account.Email, account.Email));
            emailMessage.Subject = "Change Password";

            string ChangePasswordToken = _tokensRepo.generateChangePasswordJWT((int)account.AccountId, _configuration["Jwt:Key"]);

            string ChnagePasswordEmail = GetPasswordChangeEmailTemplate($"https://FrontEndDemo.com/Security/ChangePassword?token={ChangePasswordToken}"); // the Client side (frontEnd) then sends the auth token via the Authorization Bearer

            var bodyBuilder = new BodyBuilder { HtmlBody = ChnagePasswordEmail };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {

                    client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);

                    client.Authenticate(_emailSettings.Username, _emailSettings.Password);

                    await client.SendAsync(emailMessage);
                    client.Disconnect(true);
                }
                return new CustomResponse<bool>(201, "Change password email sent successfully");
            }
            catch (Exception ex)
            {
                return new CustomResponse<bool>(500, ex.Message);
            }

        }

        public async Task<CustomResponse<bool>> ChangePassword(int accountId, string newPassword, string repeatNewPassword)
        {

            if (!_utilitiesRepo.IsValidPassword(newPassword))
                return new CustomResponse<bool>(400, "Password must be at least 8 character long with at least 1 capital letter one small and a number.");

            if (newPassword != repeatNewPassword)
                return new CustomResponse<bool>(400, "Password repitation does not match");



            Account account = await _context.Accounts.FirstOrDefaultAsync(acc => acc.AccountId == accountId);
            if (account == null)
                return new CustomResponse<bool>(404, "Account not found");

            if (_passwordService.VerifyPassword(account, newPassword))
                return new CustomResponse<bool>(409, "new password must be difference from the old password");

            account.Password = _passwordService.HashPassword(account, newPassword);

            if (!account.Verified)
                account.Verified = true;

            try
            {
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync(accountId, DateTime.UtcNow);
                return new CustomResponse<bool>(200, "Password Changed successfuuly");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong please try again later");
            }
        }

        private string GetVerifcaitonEmailTemplate(string verificationLink)
        {
            return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Email Verification</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                    -webkit-text-size-adjust: none;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                }}
                .header img {{
                    max-width: 100px;
                }}
                .content {{
                    padding: 20px;
                    text-align: center;
                }}
                .content h1 {{
                    color: #333333;
                }}
                .content p {{
                    color: #666666;
                    font-size: 16px;
                }}
                .button {{
                    display: inline-block;
                    margin: 20px 0;
                    padding: 10px 20px;
                    color: #ffffff;
                    background-color: #007bff;
                    border-radius: 5px;
                    text-decoration: none;
                    font-size: 16px;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #999999;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <img src=""C:\Users\mahmo\OneDrive\Desktop\Logo.png"" alt=""Your Logo"">
                </div>
                <div class=""content"">
                    <h1>Email Verification</h1>
                    <p>Thank you for registering with our service. Please click the button below to verify your email address.</p>
                    <a href=""{verificationLink}"" class=""button"">Verify Email</a>
                    <p>If you did not create an account, please ignore this email.</p>
                </div>
                <div class=""footer"">
                    &copy; 2024 College system. All rights reserved.
                </div>
            </div>
        </body>
        </html>";
        }


        private string GetPasswordChangeEmailTemplate(string verificationLink)
        {
            return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Email Verification</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                    -webkit-text-size-adjust: none;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                }}
                .header img {{
                    max-width: 100px;
                }}
                .content {{
                    padding: 20px;
                    text-align: center;
                }}
                .content h1 {{
                    color: #333333;
                }}
                .content p {{
                    color: #666666;
                    font-size: 16px;
                }}
                .button {{
                    display: inline-block;
                    margin: 20px 0;
                    padding: 10px 20px;
                    color: #ffffff;
                    background-color: #007bff;
                    border-radius: 5px;
                    text-decoration: none;
                    font-size: 16px;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #999999;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <img src=""C:\Users\mahmo\OneDrive\Desktop\Logo.png"" alt=""Your Logo"">
                </div>
                <div class=""content"">
                    <h1>Change password request</h1>
                    <p>Please click the button below to change your password.</p>
                    <a href=""{verificationLink}"" class=""button"">Change password</a>
                    <p>If you did not submit this request, please ignore this email.</p>
                </div>
                <div class=""footer"">
                    &copy; 2024 College system. All rights reserved.
                </div>
            </div>
        </body>
        </html>";
        }


    }
}
