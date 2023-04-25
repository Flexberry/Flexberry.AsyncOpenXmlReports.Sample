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
        /// <summary>
        /// От кого отправляются уведомления.
        /// </summary>
        private const string EmailFromValue = "vchurekov@neoplatform.ru";

        private readonly IHubContext<SignalRHub> hubContext;

        private readonly IEmailSender emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersReportsNotifier"/> class.
        /// </summary>
        /// <param name="hubContext">Зависимость SignalR. Разрешается автоматически, без Unity.</param>
        /// <param name="emailSender">Сервис отправки почты.</param>
        public UsersReportsNotifier(IHubContext<SignalRHub> hubContext, IEmailSender emailSender)
        {
            this.hubContext = hubContext;
            this.emailSender = emailSender;
        }

        /// <inheritdoc/>
        public Task SendNotifyMessage(string username, string message)
        {
            var signalID = SignalRHub.GetUserID(username);

            if (signalID != null)
            {
                return hubContext.Clients.Client(signalID).SendAsync("NotifyUser", $"{username}! {message}");
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SendReportCompleteMessage(string username, string message, string email)
        {
            var signalID = SignalRHub.GetUserID(username);

            if (signalID != null)
            {
                return hubContext.Clients.Client(signalID).SendAsync("ReportComplete", $"{username}! {message}");
            }
            else
            {
                emailSender.SendEmail(EmailFromValue, email, null, message, message, null, null, null);
            }

            return Task.CompletedTask;
        }
    }
}
