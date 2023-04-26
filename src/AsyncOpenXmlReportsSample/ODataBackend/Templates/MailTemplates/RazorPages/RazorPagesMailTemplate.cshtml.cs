namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates.RazorPages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class RazorPagesMailTemplateModel : PageModel
    {
        /// <summary>
        /// Текст сообщения в формате HTML.
        /// </summary>
        public string HtmlMessage { get; set; }
    }
}
