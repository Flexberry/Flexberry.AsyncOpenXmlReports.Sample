namespace IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates
{
    public partial class T4MailTemplate
    {
        private string _greetings;
        private string _signature;

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
        /// Подпись в письме.
        /// </summary>
        public string Signature
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_signature))
                {
                    _signature = "С наилучшими пожеланиями!";
                }

                return _signature;
            }

            set
            {
                _signature = value;
            }
        }
    }
}
