namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates.RazorPages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Модель данных представления.
    /// </summary>
    public class RazorPagesMailTemplate : PageModel
    {
        /// <summary>
        /// Текст сообщения в формате HTML.
        /// </summary>
        public string HtmlMessage { get; set; }
    }
}
