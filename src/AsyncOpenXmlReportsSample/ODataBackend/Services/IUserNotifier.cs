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
        public Task SendNotifyMessage(string username, string message);
    }
}
