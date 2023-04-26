namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System.Collections.Concurrent;
    using System.Linq;

    /// <summary>
    /// Сервис сессий пользователей.
    /// </summary>
    public class SignarRClientsService : ISignarRClientsService
    {
        private readonly ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();

        /// <inheritdoc/>
        public bool AddUser(string userId, string userName)
        {
            if (!users.ContainsKey(userId))
            {
                return users.TryAdd(userId, userName);
            }

            return false;
        }

        /// <inheritdoc/>
        public string GetUserID(string userName)
        {
            if (users.Values.Contains(userName))
                return users.First(p => p.Value == userName).Key;

            return null;
        }

        /// <inheritdoc/>
        public string GetUserName(string userId)
        {
            if (users.ContainsKey(userId))
                return users[userId];

            return null;
        }

        /// <inheritdoc/>
        public bool RemoveUser(string userId)
        {
            if (users.ContainsKey(userId))
            {
                return users.TryRemove(userId, out _);
            }

            return false;
        }
    }
}
