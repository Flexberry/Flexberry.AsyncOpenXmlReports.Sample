using System;
using System.Collections.Generic;
using System.Text;

namespace Flexberry.Quartz.Sample.Service
{
    /// <summary>
    /// Сервис текущего пользователя с использованием ролей.
    /// </summary>
    public class AdapterUserService : IUserWithRoles
    {
        /// <summary>
        /// Логин пользователя ("vpupkin").
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Имя пользователя ("VASYA Pupkin").
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Роли пользователя в строке, через запятую.
        /// </summary>
        public string Roles { get; set; }
    }
}
