namespace IIS.AsyncOpenXmlReportsSample.Services
{
    /// <summary>
    /// Интерфейс сессий пользователей.
    /// </summary>
    public interface ISignarRClientsService
    {
        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Был ли добавлен пользователь.</returns>
        public bool AddUser(string userId, string userName);

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Был ли удален пользователь.</returns>
        public bool RemoveUser(string userId);

        /// <summary>
        /// Получить идентификатор пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Идентификатор пользователя.</returns>
        public string GetUserID(string userName);

        /// <summary>
        /// Получить имя пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Имя пользователя.</returns>
        public string GetUserName(string userId);
    }
}
