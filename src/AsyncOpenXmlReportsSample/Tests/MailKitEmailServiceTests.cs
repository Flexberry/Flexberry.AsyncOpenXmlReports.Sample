namespace IIS.AsyncOpenXmlReportsSample
{
    using System.Collections.Generic;
    using System.Text;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.Services;
    using MailKit.Net.Smtp;
    using Xunit;

    public class MailKitEmailServiceTests
    {
        private string From { get; set; } = "bannikov.ru@inbox.ru";

        private string Password { get; set; } = "7qtnTN8a00fWmP1mQWv3";

        private string To { get; set; } = "rbannikov@neoplatform.ru";

        private string CopyTo { get; set; } = string.Empty;

        private EmailOptions GetEmailOptions()
        {
            return new EmailOptions
            {
                Host = "smtp.mail.ru",
                Port = 465,
                Login = this.From,
                Password = this.Password,
                EnableSsl = true,
                CheckCertificateRevocation = false,
            };
        }

        private MailKitEmailService GetMailKitEmailService()
        {
            return new MailKitEmailService(this.GetEmailOptions(), null);
        }

        private string GetEmailMessage()
        {
            return $"Настоящим письмом сообщаем об успешном запуске почтового сервиса. " +
                    $"Настоятельно рекомендуем проверить наличие прикреплённого файла." +
                    $"<br><br>" +
                    $"Мы рады, что у Вас всё получилось!<br>" +
                    $"<br><br>";
        }

        private Dictionary<string, string> GetMessageAttachments()
        {
            Dictionary<string, string> base64codeImagesArray = new Dictionary<string, string>();
            base64codeImagesArray.Add("logoIconCid", "iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAMAAADDpiTIAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAACaEAAAmhAGgBivRAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAAvFQTFRF////HR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bHR0bKEEE1AAAAPp0Uk5TAAECAwQFBgcICQoLDA0ODxAREhMU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");

            return base64codeImagesArray;
        }

        private byte[] GetFileBody()
        {
            string s = "Раз содержимое этого файла предстало перед Вашим взором, " +
                        "то мы поздравляем с очередным успехом!\n" +
                        "Верьте в себя и у Вас всё обязательно получится!";
            byte[] bitString = Encoding.UTF8.GetBytes(s);

            return bitString;
        }

        [Fact]
        public void ErrorSendingT4Email()
        {
            // Arrange
            string subject = "Тест отправки письма";
            string from = this.From;
            string to = this.To;
            string copyTo = this.CopyTo;
            string message = this.GetEmailMessage();
            Dictionary<string, string> messageAttachments = this.GetMessageAttachments();

            string fileName = "attachment.txt";
            byte[] fileBody = this.GetFileBody();
            MailKitEmailService emailService = this.GetMailKitEmailService();

            var ex = Assert.ThrowsAny<SmtpCommandException>(
                () => emailService.SendT4Email(from, to, copyTo, subject, message, messageAttachments, fileName, fileBody));

            Assert.IsType<SmtpCommandException>(ex);
        }

        [Fact]
        public async System.Threading.Tasks.Task ErrorSendingRazorEmail()
        {
            // Arrange
            string subject = "Тест отправки письма";
            string from = this.From;
            string to = this.To;
            string copyTo = this.CopyTo;
            string message = this.GetEmailMessage();
            Dictionary<string, string> messageAttachments = this.GetMessageAttachments();

            string fileName = "attachment.txt";
            byte[] fileBody = this.GetFileBody();
            MailKitEmailService emailService = this.GetMailKitEmailService();

            System.IO.Directory.SetCurrentDirectory("D:\\Work\\Projects\\Flexberry.AsyncOpenXmlReports.Sample\\src\\AsyncOpenXmlReportsSample\\ODataBackend\\");

            var ex = await Assert.ThrowsAnyAsync<SmtpCommandException>(
                () => emailService.SendRazorPagesEmail(from, to, copyTo, subject, message, messageAttachments, fileName, fileBody));

            Assert.IsType<SmtpCommandException>(ex);
        }
    }
}
