namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates.RazorPages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class RazorPagesMailTemplateModel : PageModel
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
