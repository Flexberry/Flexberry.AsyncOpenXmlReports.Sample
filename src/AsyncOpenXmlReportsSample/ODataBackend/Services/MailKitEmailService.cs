namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents;
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

        public void SendEmail(MailDataBase mailData, AttachmentFile attachmentFile)
        {
            try
            {
                using (var client = GetSmtpClient())
                {
                    var message = GetMessage(mailData, attachmentFile);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(
                    $"{nameof(MailKitEmailService)}" +
                    $". {nameof(SendEmail)}. Ошибка при отправке сообщения.", ex);
                Console.WriteLine(ex);
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

        /// <summary>
        /// Сформировать сообщение.
        /// </summary>
        /// <param name="mailData">Данные письма: от кого, кому и пр.</param>
        /// <param name="attachmentFile">Прикрепленный файл.</param>
        /// <returns>Сообщение для отправки.</returns>
        private MimeMessage GetMessage(MailDataBase mailData, AttachmentFile attachmentFile)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(mailData.From.Name, mailData.From.Email));
            foreach (var itemTo in mailData.To)
            {
                message.To.Add(new MailboxAddress(itemTo.Name, itemTo.Email));
            }

            if (mailData.CopyTo != null)
            {
                foreach (var itemCopyTo in mailData.CopyTo)
                {
                    message.Cc.Add(new MailboxAddress(itemCopyTo.Name, itemCopyTo.Email));
                }
            }

            message.Subject = mailData.Subject;

            var builder = new BodyBuilder();
            builder.TextBody = mailData.Body;
            builder.HtmlBody = mailData.Body;

            if (mailData.Resources != null)
            {
                builder = AddResources(builder, mailData.Resources);
            }

            builder.Attachments.Add(attachmentFile.Name, attachmentFile.Body);
            message.Body = builder.ToMessageBody();

            return message;
        }

        /// <summary>
        /// Добавить необходимые ресурсы (изображения) для полценного отображения письма.
        /// </summary>
        /// <param name="builder">Конструктор письма.</param>
        /// <param name="resources">Прикрепляемые ресурсы.</param>
        /// <returns></returns>
        private BodyBuilder AddResources(BodyBuilder builder, List<Resource> resources) 
        {
            foreach (Resource resource in resources)
            {
                var image = builder
                    .LinkedResources
                    .Add(resource.Id, resource.Content, resource.ContentType);
                image.ContentId = resource.Id;
            }

            return builder;
        }
    }
}
