using MailKit.Net.Smtp;
using MimeKit;


namespace Talkable.Services
{
    public class EmailService
    {
        private string message { get; } = $@"
<div dir='rtl' style='font-family: Tahoma, Arial, sans-serif; max-width: 500px; margin: 0 auto; border: 1px solid #eee; border-radius: 10px; padding: 20px; text-align: center;'>
    <h2 style='color: #673AB7;'>منصة Talkable</h2>
    <p style='font-size: 16px; color: #555;'>مرحباً بك،</p>
    <p style='font-size: 14px; color: #777;'>لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بحسابك على <strong>Talkable</strong>.</p>
    
    <div style='background-color: #f9f9f9; padding: 15px; border-radius: 8px; margin: 20px 0;'>
        <p style='font-size: 12px; color: #999; margin-bottom: 5px;'>رمز التحقق (OTP) الخاص بك هو:</p>
        <h1 style='color: #673AB7; letter-spacing: 5px; margin: 0;'>Code</h1>
    </div>

    <p style='font-size: 13px; color: #888;'>هذا الكود صالح لمدة 5 دقائق فقط.</p>
    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'>
    <p style='font-size: 11px; color: #aaa;'>إذا لم تطلب هذا الرمز، يمكنك تجاهل هذا الإيميل بأمان.</p>
    <p style='font-size: 12px; color: #673AB7; font-weight: bold;'>فريق عمل Talkable</p>
</div>";
        public async Task SendEmailAsync(string to, string subject, string code)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("moamenragab66@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Replace("Code", code)  
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("moamenragab66@gmail.com", "lltzexpbvqguzoeh");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
