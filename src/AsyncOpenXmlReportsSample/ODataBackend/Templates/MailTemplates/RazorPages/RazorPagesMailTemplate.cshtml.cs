namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates.RazorPages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class RazorPagesMailTemplateModel : PageModel
    {
        /// <summary>
        /// ����� ��������� � ������� HTML.
        /// </summary>
        public string HtmlMessage { get; set; }
    }
}
