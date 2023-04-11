namespace IIS.AsyncOpenXmlReportsSample
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб SignalR. Это объект через который происходит взаимодействие с клиентами.
    /// </summary>
    public class SignalRHub : Hub
    {
        /// <summary>
        /// Методы вызывается из клиента (ember-фронтенда) SignalR. Выполняет вызов метода на стороне клиента.
        /// </summary>
        /// <param name="username">Имя пользователя, для информации.</param>
        public async Task SendNotifyUserMessage(string username)
        {
            await Clients.All.SendAsync("NotifyUser", $"Hi {username}");
        }
    }
}
