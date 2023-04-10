namespace IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents
{
    using MimeKit;

    /// <summary>
    /// Ресурс, отображаемый в письме.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Идентификатор ресурса.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Содержимое ресурса.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Тип содержимого ресурса.
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="id">Идентификатор ресурса.</param>
        /// <param name="content">Содержимое ресурса.</param>
        /// <param name="contentType">Тип содержимого ресурса.</param>
        public Resource(string id, byte[] content, ContentType contentType)
        {
            this.Id = id;
            this.Content = content;
            this.ContentType = contentType;
        }
    }
}
