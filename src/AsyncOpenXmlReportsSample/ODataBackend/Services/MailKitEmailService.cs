namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates;
    using IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates.RazorPages;
    using MailKit.Net.Smtp;
    using MimeKit;

    /// <summary>
    /// Сервис отправки Email-сообщений с помощью библиотеки MailKit.
    /// </summary>
    public class MailKitEmailService : IEmailSender
    {
        /// <summary>
        /// Настройки отправки по email.
        /// </summary>
        private readonly EmailOptions _emailOptions;

        /// <summary>
        /// Рендер Razor Page в строковое представление.
        /// </summary>
        private readonly IRazorViewToStringRenderer _razorPageToStringRenderer;

        public MailKitEmailService(EmailOptions emailOptions, IRazorViewToStringRenderer razorPageToStringRenderer)
        {
            this._emailOptions = emailOptions;
            this._razorPageToStringRenderer = razorPageToStringRenderer;
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Razor Pages.
        /// </summary>
        /// <param name="from">Отправитель письма.</param>
        /// <param name="to">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        public async Task SendRazorPagesEmail(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            var mailModel = new RazorPagesMailTemplateModel() 
            {
                HtmlMessage = body,
            };

            string razorPage = "/Templates/MailTemplates/RazorPages/RazorPagesMailTemplate.cshtml";
            string messageBody = await _razorPageToStringRenderer.RenderViewToStringAsync(razorPage, mailModel);
            this.SendEmail(from, to, copyTo, subject, messageBody, bodyAttachments, fileName, fileBody);
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Т4.
        /// </summary>
        /// <param name="from">Отправитель письма.</param>
        /// <param name="to">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        public void SendT4Email(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            T4MailTemplate mailCommonRu = new T4MailTemplate()
            {
                HtmlMessage = body,
            };
            string messageBody = mailCommonRu.TransformText();
            this.SendEmail(from, to, copyTo, subject, messageBody, bodyAttachments, fileName, fileBody);
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Т4.
        /// </summary>
        /// <param name="from">Отправитель письма.</param>
        /// <param name="to">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        public void SendEmail(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            try
            {
                using (var client = this.GetSmtpClient())
                {
                    var message = this.GetMessage(from, to, copyTo, subject, body, bodyAttachments, fileName, fileBody);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(
                    $"{nameof(MailKitEmailService)}" +
                    $". {nameof(this.SendEmail)}. Ошибка при отправке сообщения.", ex);
                throw;
            }
        }

        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            LogService.LogInfo($"{nameof(MailKitEmailService)} Connect to SmtpClient {this._emailOptions.Host} by port {_emailOptions.Port}");
            LogService.LogInfo($"{nameof(MailKitEmailService)} CheckCertificateRevocation =  {this._emailOptions.CheckCertificateRevocation}");
            client.CheckCertificateRevocation = this._emailOptions.CheckCertificateRevocation;

            client.Connect(this._emailOptions.Host, this._emailOptions.Port, this._emailOptions.EnableSsl);

            if (!string.IsNullOrEmpty(this._emailOptions.Login) && !string.IsNullOrEmpty(this._emailOptions.Password))
            {
                client.Authenticate(this._emailOptions.Login, this._emailOptions.Password);
            }

            return client;
        }

        private MimeMessage GetMessage(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));

            if (!string.IsNullOrEmpty(copyTo))
            {
                message.Cc.Add(new MailboxAddress(copyTo, copyTo));
            }

            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = body;
            builder.HtmlBody = body;

            if (bodyAttachments != null)
            {
                builder = AddResources(builder, bodyAttachments);
            }

            if (fileBody != null)
            {
                builder.Attachments.Add(fileName, fileBody);
            }

            message.Body = builder.ToMessageBody();

            return message;
        }

        /// <summary>
        /// Добавить необходимые ресурсы (изображения) для полценного отображения письма.
        /// </summary>
        /// <param name="builder">Конструктор письма.</param>
        /// <param name="bodyAttachments">Прикрепляемые изображения.</param>
        /// <returns>Контруктор письма с прикрепленными изображениями к содержимому.</returns>
        private static BodyBuilder AddResources(BodyBuilder builder, Dictionary<string, string> bodyAttachments)
        {
            foreach (KeyValuePair<string, string> img in bodyAttachments)
            {
                byte[] bitmapData = Convert.FromBase64String(img.Value);
                var image = builder
                    .LinkedResources
                    .Add(img.Key, bitmapData);
                image.ContentId = img.Key;
            }

            return builder;
        }
    }
}
