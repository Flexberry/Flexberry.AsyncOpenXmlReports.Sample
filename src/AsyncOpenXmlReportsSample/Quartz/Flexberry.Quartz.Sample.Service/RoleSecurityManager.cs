namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Security;
    using NewPlatform.Flexberry.Caching;
    using NewPlatform.Flexberry.Security;

    /// <summary>
    /// SecurityManager на основе ролей.
    /// </summary>
    public class RoleSecurityManager : ISecurityManager
    {
        /// <summary>
        /// Текущий пользователь с ролями.
        /// </summary>
        private readonly IUserWithRoles currentUser;

        /// <summary>
        /// Определение языка ограничений для конструирования ограничивающих функций.
        /// </summary>
        /// <remarks>
        /// Использует сервис данных менеджера полномочий.
        /// При создании функций ограничения следует использовать именно данную инстанцию,
        /// а не статическое свойство <c>ExternalLangDef.LanguageDef</c>.
        /// </remarks>
        private readonly ICSSoft.STORMNET.Windows.Forms.ExternalLangDef langDef;

        /// <summary>
        /// Сервис данных, использующийся для загрузки объектов полномочий.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Кеш для данной инстанции <see cref="SecurityManager"/>.
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// Инициализация сервиса полномочий на основе ролей.
        /// </summary>
        /// <param name="roles">Список ролей.</param>
        /// <param name="dataService">Сервис данных для загрузки информации о полномочиях.</param>
        /// <param name="cacheService">Сервис кеширования.</param>
        /// <param name="user">Сервис текущего пользователя.</param>
        public RoleSecurityManager(IDataService dataService, ICacheService cacheService, IUserWithRoles user)
        {
            this.currentUser = user;
            this.dataService = dataService;
            this.cacheService = cacheService;
            langDef = new ICSSoft.STORMNET.Windows.Forms.ExternalLangDef { DataService = dataService };
        }

        /// <summary>
        /// Флаг включенных полномочий.
        /// </summary>
        /// <remarks>
        /// В данной реализации не используется.
        /// </remarks>
        public bool Enabled { get; }

        /// <summary>
        /// Флаг включенных полномочий над объектами.
        /// </summary>
        /// <remarks>
        /// В данной реализации не используется.
        /// </remarks>
        public bool UseRightsOnObjects { get; }

        /// <summary>
        /// Флаг включенных полномочий над атрибутами.
        /// </summary>
        /// <remarks>
        /// В данной реализации не используется.
        /// </remarks>
        public bool UseRightsOnAttribute { get; }

        /// <summary>
        /// Регулярное выражение для извлечения информации о контроле прав на атрибуты из <see cref="DataServiceExpressionAttribute"/>.
        /// </summary>
        public string AttributeCheckExpressionPattern => @"/\*Operation:(?<Operation>.*);DeniedAccessValue:(?<DeniedAccessValue>.*)\*/";

        /// <summary>
        /// Проверка полномочий на выполнение операции.
        /// </summary>
        /// <remarks>Проверяется только наличие записи, но не тип доступа.</remarks>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// </returns>
        public bool AccessCheck(int operationId)
        {
            return AccessCheck(operationId.ToString());
        }

        /// <summary>
        /// Проверка полномочий на выполнение операции.
        /// </summary>
        /// <remarks>Проверяется только наличие записи, но не тип доступа.</remarks>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// </returns>
        public bool AccessCheck(string operationId)
        {
            bool result = false;
            try
            {
                if (!(currentUser?.RolesList?.Any() ?? false))
                    return false;

                foreach (var roleName in currentUser.RolesList)
                {
                    string cacheKey = $"{nameof(AccessCheck)}_RoleName_{roleName}_{operationId}";

                    // Проверим в кэше.
                    if (cacheService != null && cacheService.Exists(cacheKey))
                    {
                        if (cacheService.GetFromCache<int>(cacheKey) > 0)
                            return true;
                    }
                    else
                    {
                        // Ограничение на роли по имени.
                        var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Agent), Agent.Views.RoleOrGroupL);

                        lcs.LimitFunction = langDef.GetFunction(
                                langDef.funcAND,
                                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Agent>(x => x.Name)), roleName),
                                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.BoolType, Information.ExtractPropertyPath<Agent>(x => x.IsRole))));

                        var roles = dataService.LoadObjects(lcs).Cast<Agent>();

                        // Для каждой роли вычитаем дочерние роли и проверим наличие доступа.
                        foreach (var role in roles)
                        {
                            var agentPKs = new List<Guid>();

                            // Вычитаем дочерние роли.
                            AddLinkedAgents(role.AgentKey.Value, agentPKs);

                            // Ограничение на все дочерние роли.
                            var funcInParams = new object[agentPKs.Count + 1];
                            funcInParams[0] = new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<Permition>(x => x.Agent));

                            for (int i = 0; i < agentPKs.Count; i++)
                            {
                                funcInParams[i + 1] = agentPKs[i];
                            }

                            lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Permition), Permition.Views.CheckAccessOperation);
                            lcs.LimitFunction = langDef.GetFunction(
                                langDef.funcAND,
                                langDef.GetFunction(langDef.funcIN, funcInParams),
                                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.BoolType, Information.ExtractPropertyPath<Permition>(x => x.Subject.IsOperation))),
                                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Permition>(x => x.Subject.Name)), operationId));

                            // Количество ролей которым доступна указанная операция.
                            var permitionsCount = dataService.GetObjectsCount(lcs);

                            cacheService?.SetToCache(cacheKey, permitionsCount, new string[] { SecurityCacheTags.SecurityCacheMainTag, SecurityCacheTags.SecurityManagerCacheTag });

                            if (permitionsCount > 0)
                                return true;
                        }
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                LogService.LogError($"Error in checking access to operation {operationId}.", exc);
                throw;
            }
        }

        /// <summary>
        /// Проверка полномочий на выполнение операции с типом.
        /// </summary>
        /// <param name="type">Тип объекта данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение в случае отсутствия прав.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// Возвращает <c>true</c> без проверок если полномочия выключены в <see cref="Enabled"/>.
        /// </returns>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="type"/>.</exception>
        /// <exception cref="ICSSoft.STORMNET.UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        public bool AccessObjectCheck(Type type, tTypeAccess operation, bool throwException)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка полномочий на выполнение операции с объектом.
        /// </summary>
        /// <param name="obj">Объект данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение в случае отсутствия прав.</param>
        /// <returns>Если у текущего пользователя есть доступ, то <c>true</c>.</returns>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="obj"/>.</exception>
        /// <exception cref="ICSSoft.STORMNET.UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        public bool AccessObjectCheck(object obj, tTypeAccess operation, bool throwException)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод проверки прав на доступ текущего пользователя к операции, заданной в <see cref="DataServiceExpressionAttribute"/> атрибута.
        /// </summary>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <param name="expression">Строка <see cref="DataServiceExpressionAttribute"/>.</param>
        /// <param name="deniedAccessValue">Значение, которое должен получить атрибут при отсутствии прав.</param>
        /// <returns>Если у текущего пользователя есть доступ, то <c>true</c>.</returns>
        public bool CheckAccessToAttribute(string expression, out string deniedAccessValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить наличие в системе логина (чувствительность к регистру зависит от настроек источника данных).
        /// Уникальность проверяется без контроля доменов, то есть гарантируется уникальность в рамках всей таблицы.
        /// Отключенные пользователи тоже учитываются, как занимающие логин.
        /// </summary>
        /// <param name="login">Логин, который проверяем.</param>
        /// <returns>Если логин свободен, то <see cref="OperationResult.ЛогинСвободен"/>, если занят, то <see cref="OperationResult.ЛогинЗанят"/>.</returns>
        /// <exception cref="ArgumentException">Исключение генерируется при передаче <c>null</c> или <c>string.Empty</c> в качестве значения для <paramref name="login"/>.</exception>
        public OperationResult CheckExistLogin(string login)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login), "Login can not be null.");

            if (login == string.Empty)
                throw new ArgumentException("Login can not be an empty string.");

            return login == currentUser.Login ? OperationResult.ЛогинЗанят : OperationResult.ЛогинСвободен;
        }

        /// <summary>
        /// Получить ограничение для текущего пользователя.
        /// </summary>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <param name="subjectType">Тип объекта.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="limit">Ограничение, которое есть для текущего пользователя.</param>
        /// <param name="canAccess">Есть ли доступ к этому типу у пользователя.</param>
        /// <returns>Результат выполнения операции.</returns>
        public OperationResult GetLimitForAccess(Type subjectType, tTypeAccess operation, out object limit, out bool canAccess)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить роли с заданными ограничениями, которые реализуют функцию разграничения по объектам.
        /// </summary>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <param name="type">Класс, для которого получаем ограничения.</param>
        /// <param name="rolesWithAccesses">Роли с заданными ограничениями для этих ролей.</param>
        /// <returns>Результат выполнения операции.</returns>
        public OperationResult GetLimitStrForRoles(Type type, out List<RoleWithAccesses> rolesWithAccesses)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Задать ограничение для указанной роли.
        /// </summary>
        /// <remarks>
        /// Не поддерживается в текущей реализации.
        /// </remarks>
        /// <param name="type">Тип объектов данных, для которых будет применяться данный фильтр.</param>
        /// <param name="operation">Тип доступа, для которого применяется этот фильтр.</param>
        /// <param name="roleName">Название роли.</param>
        /// <param name="filter">Сериализованный фильтр, который будет применяться для указанной роли.</param>
        /// <returns>Результат выполнения операции.</returns>
        public OperationResult SetLimitStrForRole(Type type, tTypeAccess operation, string roleName, string filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод для очистки кеша менеджера полномочий.
        /// </summary>
        public void ClearCache()
        {
            cacheService.ClearCache();
        }

        /// <summary>
        /// Добавить прилинкованных агентов.
        /// </summary>
        /// <param name="userKey">Идентификатор агента, для которого формируется список всех связанных.</param>
        /// <param name="agentPKs">Коллекция прилинкованных агентов.</param>
        private void AddLinkedAgents(Guid userKey, ICollection<Guid> agentPKs)
        {
            // Чтобы избежать циклической обработки, если вдруг ссылки будут стоять друг на друга.
            if (!agentPKs.Contains(userKey))
            {
                agentPKs.Add(userKey);

                // Если это не было сделано ранее, загрузим кеш.
                string linkGroupCacheKey = $"Security_LinkGroups";
                Dictionary<Guid, LinkGroup> linkGroupCache;

                if (cacheService == null || !cacheService.Exists(linkGroupCacheKey))
                {
                    linkGroupCache = new Dictionary<Guid, LinkGroup>();
                    LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(LinkGroup), LinkGroup.Views.LinkedAgents);

                    DataObject[] objs = dataService.LoadObjects(lcs);
                    foreach (LinkGroup link in objs)
                    {
                        linkGroupCache.Add(((KeyGuid)link.__PrimaryKey).Guid, link);
                    }

                    cacheService?.SetToCache(linkGroupCacheKey, linkGroupCache, new string[] { SecurityCacheTags.SecurityCacheMainTag, SecurityCacheTags.SecurityManagerCacheTag });
                }
                else
                {
                    linkGroupCache = cacheService.GetFromCache<Dictionary<Guid, LinkGroup>>(linkGroupCacheKey);
                }

                string linkRoleCacheKey = $"Security_LinkRoles";
                Dictionary<Guid, LinkRole> linkRoleCache;

                if (cacheService == null || !cacheService.Exists(linkRoleCacheKey))
                {
                    linkRoleCache = new Dictionary<Guid, LinkRole>();
                    LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(LinkRole), LinkRole.Views.LinkedAgents);

                    DataObject[] objs = dataService.LoadObjects(lcs);
                    foreach (LinkRole link in objs)
                    {
                        linkRoleCache.Add(((KeyGuid)link.__PrimaryKey).Guid, link);
                    }

                    cacheService?.SetToCache(linkRoleCacheKey, linkRoleCache, new string[] { SecurityCacheTags.SecurityCacheMainTag, SecurityCacheTags.SecurityManagerCacheTag });
                }
                else
                {
                    linkRoleCache = cacheService.GetFromCache<Dictionary<Guid, LinkRole>>(linkRoleCacheKey);
                }

                // Группы.
                foreach (LinkGroup link in linkGroupCache.Values)
                {
                    if (((KeyGuid)link.User.__PrimaryKey).Guid == userKey)
                    {
                        AddLinkedAgents(((KeyGuid)link.Group.__PrimaryKey).Guid, agentPKs);
                    }
                }

                // Роли.
                foreach (LinkRole link in linkRoleCache.Values)
                {
                    if (((KeyGuid)link.Agent.__PrimaryKey).Guid == userKey)
                    {
                        AddLinkedAgents(((KeyGuid)link.Role.__PrimaryKey).Guid, agentPKs);
                    }
                }
            }

            if (!agentPKs.Contains(Agent.AllUsersRoleGuid))
                agentPKs.Add(Agent.AllUsersRoleGuid);
        }
    }
}
