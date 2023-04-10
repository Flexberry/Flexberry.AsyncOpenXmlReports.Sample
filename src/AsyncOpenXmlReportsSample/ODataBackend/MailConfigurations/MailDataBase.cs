namespace IIS.AsyncOpenXmlReportsSample.MailConfigurations
{
    using System.Collections.Generic;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents;
    using IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates;

    public class MailDataBase
    {
        /// <summary>
        /// Контактные данные отправителя письма.
        /// </summary>
        public Contact From { get; set; }

        /// <summary>
        /// Контактные данные получателей письма.
        /// </summary>
        public List<Contact> To { get; set; }

        /// <summary>
        /// Контактные данные олучателей копии письма.
        /// </summary>
        public List<Contact> CopyTo { get; set; }

        /// <summary>
        /// Тема письма.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Содержимое письма.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Прикрепляемые ресурсы для полноценного отображения письма (изображения).
        /// </summary>
        public List<Resource> Resources { get; set; } = T4MailTemplateResources.GetT4MailTemplateResources();
    }
}
