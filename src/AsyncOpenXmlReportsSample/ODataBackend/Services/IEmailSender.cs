namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents;

    /// <summary>
    /// Интерфейс сервиса отправки почты.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправить письмо.
        /// </summary>
        /// <param name="mailData">Данные письма: от кого, кому и пр.</param>
        /// <param name="attachmentFile">Прикрепленный файл.</param>
        void SendEmail(MailDataBase mailData, AttachmentFile attachmentFile);
    }
}
