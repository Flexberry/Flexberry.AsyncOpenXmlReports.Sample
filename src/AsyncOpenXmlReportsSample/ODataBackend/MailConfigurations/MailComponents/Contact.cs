namespace IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents
{
    /// <summary>
    /// Контактные данные.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Имя контакта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="name">Имя контакта.</param>
        /// <param name="email">Электронная почта.</param>
        public Contact(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
    }
}
