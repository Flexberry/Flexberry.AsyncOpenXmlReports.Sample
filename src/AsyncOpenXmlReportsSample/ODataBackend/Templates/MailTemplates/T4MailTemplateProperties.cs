namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates
{
    public partial class T4MailTemplate
    {
        /// <summary>
        /// Заголовок письма.
        /// </summary>
        public string HeadTitle { get; set; }

        /// <summary>
        /// Текст сообщения в формате HTML.
        /// </summary>
        public string HtmlMessage { get; set; }
    }
}
