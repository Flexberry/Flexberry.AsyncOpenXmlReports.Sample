namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Класс для оповещения пользоваетелей о результатах задачи формирования отчета.
    /// </summary>
    public class UsersReportsNotifier : IUserNotifier
    {
        private IHubContext<SignalRHub> hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersReportsNotifier"/> class.
        /// </summary>
        /// <param name="hubContext">Зависимость SignalR. Разрешается автоматически, без Unity.</param>
        public UsersReportsNotifier(IHubContext<SignalRHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        /// <summary>
        /// Отправить оповещение пользователю.
        /// </summary>
        /// <param name="username">Имя пользователя, которого нужно оповестить.</param>
        /// <param name="message">Текст оповещения.</param>
        public Task SendNotifyMessage(string username, string message)
        {
            throw new NotImplementedException();
        }
    }
}
