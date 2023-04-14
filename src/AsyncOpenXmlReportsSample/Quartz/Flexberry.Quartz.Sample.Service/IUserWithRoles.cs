namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        List<string> RolesList
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Roles))
                {
                    return new List<string>(this.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()));
                }

                return null;
            }
        }
    }
}
