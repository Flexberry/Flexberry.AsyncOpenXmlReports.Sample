namespace IIS.AsyncOpenXmlReportsSample
{
    /// <summary>
    /// Интерфейс сервиса текущего пользователя с использованием ролей.
    /// </summary>
    public interface IUserWithRolesAndEmail : ICSSoft.Services.CurrentUserService.IUser
    {
        /// <summary>
        /// Gets or sets роли пользователя в строке, через запятую.
        /// </summary>
        string Roles { get; set; }

        /// <summary>
        /// Gets or sets email пользователя для отправки ему уведомления об отчете.
        /// </summary>
        string Email { get; set; }
    }
}
