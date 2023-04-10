namespace IIS.AsyncOpenXmlReportsSample.MailConfigurations
{
    /// <summary>
    /// Настройки для отправки email-ов.
    /// </summary>
    public class EmailOptions
    {
        /// <summary>
        /// Адрес smtp-сервера.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт smtp-сервера.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Домен для авторизации на почтовом сервере.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Логин для авторизации на почтовом сервере.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль для авторизации на почтовом сервере.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Использовать SSL.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Проверять отзыв сертификата.
        /// </summary>
        public bool CheckCertificateRevocation { get; set; }
    }
}
