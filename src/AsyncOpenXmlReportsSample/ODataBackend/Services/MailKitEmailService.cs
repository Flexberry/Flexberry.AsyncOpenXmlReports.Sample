﻿namespace IIS.AsyncOpenXmlReportsSample.Services
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
        private readonly EmailOptions emailOptions;

        /// <summary>
        /// Рендер Razor Page в строковое представление.
        /// </summary>
        private readonly IRazorViewToStringRenderer razorPageToStringRenderer;

        /// <summary>
        /// Сервис отправки электронных писем.
        /// </summary>
        /// <param name="emailOptions">Настройки электронной почты.</param>
        /// <param name="razorPageToStringRenderer">Сервис рендеринга шаблонов с синтаксисом Razor.</param>
        public MailKitEmailService(EmailOptions emailOptions, IRazorViewToStringRenderer razorPageToStringRenderer)
        {
            this.emailOptions = emailOptions;
            this.razorPageToStringRenderer = razorPageToStringRenderer;
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Razor Pages.
        /// </summary>
        /// <param name="sender">Отправитель письма.</param>
        /// <param name="recipient">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        /// <returns>A <see cref="Task"/>Task.</returns>
        public async Task SendRazorPagesEmail(string sender, string recipient, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            var mailModel = new RazorPagesMailTemplate()
            {
                HtmlMessage = body,
            };

            string razorPage = "/Templates/MailTemplates/RazorPages/RazorPagesMailTemplate.cshtml";
            string messageBody = await razorPageToStringRenderer.RenderViewToStringAsync(razorPage, mailModel).ConfigureAwait(false);
            SendEmail(sender, recipient, copyTo, subject, messageBody, bodyAttachments, fileName, fileBody);
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Т4.
        /// </summary>
        /// <param name="sender">Отправитель письма.</param>
        /// <param name="recipient">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        public void SendT4Email(string sender, string recipient, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            T4MailTemplate mailCommonRu = new T4MailTemplate()
            {
                HtmlMessage = body,
            };
            string messageBody = mailCommonRu.TransformText();
            SendEmail(sender, recipient, copyTo, subject, messageBody, bodyAttachments, fileName, fileBody);
        }

        /// <summary>
        /// Отправить письмо, сформированное на основе шаблона Т4.
        /// </summary>
        /// <param name="sender">Отправитель письма.</param>
        /// <param name="recipient">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        public void SendEmail(string sender, string recipient, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            try
            {
                using (var client = GetSmtpClient())
                {
                    var message = GetMessage(sender, recipient, copyTo, subject, body, bodyAttachments, fileName, fileBody);
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

        /// <summary>
        /// Получить smtp-клиент.
        /// </summary>
        /// <returns>smtp-клиент.</returns>
        private SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            LogService.LogInfo($"{nameof(MailKitEmailService)} Connect to SmtpClient {emailOptions.Host} by port {emailOptions.Port}");
            LogService.LogInfo($"{nameof(MailKitEmailService)} CheckCertificateRevocation =  {emailOptions.CheckCertificateRevocation}");
            client.CheckCertificateRevocation = emailOptions.CheckCertificateRevocation;

            client.Connect(emailOptions.Host, emailOptions.Port, emailOptions.EnableSsl);

            if (!string.IsNullOrEmpty(emailOptions.Login) && !string.IsNullOrEmpty(emailOptions.Password))
            {
                client.Authenticate(emailOptions.Login, emailOptions.Password);
            }

            return client;
        }

        /// <summary>
        /// Получить сформированное сообщение.
        /// </summary>
        /// <param name="sender">Отправитель письма.</param>
        /// <param name="recipient">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        /// <returns>Сформированное сообщение.</returns>
        private MimeMessage GetMessage(string sender, string recipient, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender, sender));
            message.To.Add(new MailboxAddress(recipient, recipient));

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
        /// Добавить необходимые ресурсы (изображения) для полноценного отображения письма.
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
