namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс сервиса отправки почты.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправить письмо.
        /// </summary>
        /// <param name="from">Отправитель письма.</param>
        /// <param name="to">Получатель письма.</param>
        /// <param name="copyTo">Получатель копии письма.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="body">Содержимое письма.</param>
        /// <param name="bodyAttachments">Прикреляемые изображения для отображения в теле письма.</param>
        /// <param name="fileName">Имя прикрепляемого файла.</param>
        /// <param name="fileBody">Содержимое прикрепляемого файла.</param>
        void SendEmail(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody);

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
        void SendT4Email(string from, string to, string copyTo, string subject, string body, Dictionary<string, string> bodyAttachments, string fileName, byte[] fileBody);
    }
}
