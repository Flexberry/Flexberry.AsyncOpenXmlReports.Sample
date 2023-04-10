namespace IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents
{
    /// <summary>
    /// Прикрепленный файл.
    /// </summary>
    public class AttachmentFile
    {
        /// <summary>
        /// Имя файла.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Содержимое файла.
        /// </summary>
        public byte[] Body { get; set; }
    }
}
