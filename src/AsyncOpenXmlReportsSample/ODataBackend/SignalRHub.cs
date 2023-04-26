namespace IIS.AsyncOpenXmlReportsSample
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using IIS.AsyncOpenXmlReportsSample.Services;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб SignalR. Это объект через который происходит взаимодействие с клиентами.
    /// </summary>
    public class SignalRHub : Hub
    {
        private readonly ISignarRClientsService signarRClientsService;

        /// <summary>
        /// Инициализация SignalR.
        /// </summary>
        /// <param name="signarRClientsService">Сервис сессий пользователей.</param>
        public SignalRHub(ISignarRClientsService signarRClientsService)
        {
            this.signarRClientsService = signarRClientsService;
        }

        /// <summary>
        /// Подключение пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        public void AddUser(string userName)
        {
            var id = Context.ConnectionId;

            signarRClientsService.AddUser(id, userName);
        }

        /// <summary>
        /// Метод вызывается из клиента (ember-фронтенда) SignalR. Выполняет вызов метода на стороне клиента.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="message">Сообщение для пользователя.</param>
        /// <returns>Task.</returns>
        public async Task SendNotifyUserMessage(string username, string message)
        {
            var userId = signarRClientsService.GetUserID(username);

            await Clients.Client(userId).SendAsync("NotifyUser", $"{username}! {message}").ConfigureAwait(false);
        }

        /// <summary>
        /// Метод вызывается при отключении клиента.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var id = Context.ConnectionId;

            signarRClientsService.RemoveUser(id);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
