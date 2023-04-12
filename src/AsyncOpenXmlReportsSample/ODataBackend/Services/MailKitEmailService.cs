namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates;
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

        public MailKitEmailService(EmailOptions emailOptions)
        {
            _emailOptions = emailOptions;
        }

        public void SendEmail(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            try
            {
                using (var client = GetSmtpClient())
                {
                    var message = GetMessage(from, to, copyTo, subject, body, bodyAttachments, fileName, fileBody);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(
                    $"{nameof(MailKitEmailService)}" +
                    $". {nameof(SendEmail)}. Ошибка при отправке сообщения.", ex);
                throw;
            }
        }

        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            LogService.LogInfo($"{nameof(MailKitEmailService)} Connect to SmtpClient {_emailOptions.Host} by port {_emailOptions.Port}");
            LogService.LogInfo($"{nameof(MailKitEmailService)} CheckCertificateRevocation =  {_emailOptions.CheckCertificateRevocation}");
            client.CheckCertificateRevocation = _emailOptions.CheckCertificateRevocation;

            client.Connect(_emailOptions.Host, _emailOptions.Port, _emailOptions.EnableSsl);

            if (!string.IsNullOrEmpty(_emailOptions.Login) && !string.IsNullOrEmpty(_emailOptions.Password))
            {
                client.Authenticate(_emailOptions.Login, _emailOptions.Password);
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

            if (bodyAttachments == null)
            {
                bodyAttachments = T4MailTemplateAttachments.GetT4MailTemplateAttachments();
            }

            builder = AddResources(builder, bodyAttachments);

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
