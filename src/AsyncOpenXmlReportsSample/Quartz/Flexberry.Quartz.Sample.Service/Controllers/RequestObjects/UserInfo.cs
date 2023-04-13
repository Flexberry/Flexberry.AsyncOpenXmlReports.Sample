namespace Flexberry.Quartz.Sample.Service.Controllers.RequestObjects
{
    /// <summary>
    /// Информация о пользователе.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Роли пользователя, разделенные запятоыми.
        /// </summary>
        public string Roles { get; set; }
    }
}
