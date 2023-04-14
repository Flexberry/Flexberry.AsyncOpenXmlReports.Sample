namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates
{
    public partial class T4MailTemplate
    {
        /// <summary>
        /// Тема письма.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения в формате HTML.
        /// </summary>
        public string HtmlMessage { get; set; }
    }
}
