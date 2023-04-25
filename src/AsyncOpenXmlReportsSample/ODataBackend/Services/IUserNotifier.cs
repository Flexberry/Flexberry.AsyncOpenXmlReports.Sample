namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс сервиса оповещения пользоваетелей.
    /// </summary>
    public interface IUserNotifier
    {
        /// <summary>
        /// Отправить оповещение пользователю.
        /// </summary>
        /// <param name="username">Имя пользователя, которого нужно оповестить.</param>
        /// <param name="message">Текст оповещения.</param>
        /// <returns>Task.</returns>
        public Task SendNotifyMessage(string username, string message);

        /// <summary>
        /// Отправить оповещение пользователю о формировании отчета.
        /// </summary>
        /// <param name="username">Имя пользователя, которого нужно оповестить.</param>
        /// <param name="message">Текст оповещения.</param>
        /// <param name="email">Почта пользователя.</param>
        /// <returns>Task.</returns>
        public Task SendReportCompleteMessage(string username, string message, string email);
    }
}
