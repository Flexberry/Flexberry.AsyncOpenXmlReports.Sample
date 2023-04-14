namespace IIS.AsyncOpenXmlReportsSample
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб SignalR. Это объект через который происходит взаимодействие с клиентами.
    /// </summary>
    public class SignalRHub : Hub
    {
        private static ConcurrentDictionary<string, string> users;

        public SignalRHub()
        {
            if (users == null)
            {
                users = new ConcurrentDictionary<string, string>();
            }
        }

        /// <summary>
        /// Подключение пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public void AddUser(string userName)
        {
            var id = Context.ConnectionId;
            if (!users.ContainsKey(id))
            {
                users.TryAdd(id, userName);
            }
        }

        /// <summary>
        /// Метод вызывается из клиента (ember-фронтенда) SignalR. Выполняет вызов метода на стороне клиента.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="message">Сообщение для пользователя.</param>
        public async Task SendNotifyUserMessage(string username, string message)
        {
            if (users.Values.Contains(username))
            {
                await Clients.Caller.SendAsync("NotifyUser", $"{username}! {message}");
            }
        }

        /// <summary>
        /// Метод вызывается при отключении клиента.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var id = Context.ConnectionId;

            if (users.ContainsKey(id))
            {
                users.TryRemove(id, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
