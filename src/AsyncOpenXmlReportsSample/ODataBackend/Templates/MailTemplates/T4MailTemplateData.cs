
namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates
{
    /// <summary>
    /// Общий шаблон для писем.
    /// </summary>
    public partial class T4MailTemplate
    {

        private string _greetings;

        /// <summary>
        /// Тема письма.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения в формате HTML.
        /// </summary>
        public string HtmlMessage { get; set; }

        /// <summary>
        /// Приветствие в письме.
        /// </summary>
        public string Greetings
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_greetings))
                {
                    _greetings = "Здравствуйте!";
                }

                return _greetings;
            }

            set
            {
                _greetings = value;
            }
        }

        /// <summary>
        /// Адрес сайта.
        /// </summary>
        public string PublishedSiteUrl
        {
            get
            {
                return "https://flexberry.net/";
            }
        }

    }
}
