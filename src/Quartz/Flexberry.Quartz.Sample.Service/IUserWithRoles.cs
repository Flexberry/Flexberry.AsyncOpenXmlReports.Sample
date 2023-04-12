using System;
using System.Linq;
using System.Collections.Generic;

namespace Flexberry.Quartz.Sample.Service
{
    /// <summary>
    /// Интерфейс сервиса текущего пользователя с использованием ролей.
    /// </summary>
    public interface IUserWithRoles : ICSSoft.Services.CurrentUserService.IUser
    {
        /// <summary>
        /// Роли пользователя в строке, через запятую.
        /// </summary>
        string Roles { get; set; }

        /// <summary>
        /// Перечень ролей.
        /// </summary>
        List<string> RolesList {
            get {
                if (!string.IsNullOrWhiteSpace(Roles))
                {
                    return new List<string>(Roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()));
                }

                return null;
            }
        }
    }
}
