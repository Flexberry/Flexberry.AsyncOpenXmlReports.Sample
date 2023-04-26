namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Security;
    using NewPlatform.Flexberry.Caching;
    using NewPlatform.Flexberry.Security;
    using UnauthorizedAccessException = ICSSoft.STORMNET.UnauthorizedAccessException;

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
            var userRoles = GetUserRoleList();

            if (!(userRoles?.Any() ?? false))
                return false;

            bool totalResult = false;

            try
            {
                foreach (var roleName in userRoles)
                {
                    var result = false;
                    string cacheKey = $"{nameof(AccessCheck)}_RoleName_{roleName}_{operationId}";

                    // Проверим в кэше.
                    if (cacheService != null && cacheService.Exists(cacheKey))
                    {
                        if (cacheService.GetFromCache<int>(cacheKey) > 0)
                            return true;
                    }
                    else
                    {
                        IEnumerable<Agent> roles = GetRolesByName(roleName);

                        // Для каждой роли вычитаем дочерние роли и проверим наличие доступа.
                        foreach (var role in roles)
                        {
                            var agentPKs = new List<Guid>();

                            // Вычитаем дочерние роли.
                            AddLinkedAgents(role.AgentKey.Value, agentPKs);

                            // Ограничение на все дочерние роли.
                            int permitionsCount = GetPermitionCount(operationId, agentPKs);

                            cacheService?.SetToCache(cacheKey, permitionsCount, new string[] { SecurityCacheTags.SecurityCacheMainTag, SecurityCacheTags.SecurityManagerCacheTag });

                            if (permitionsCount > 0)
                                result = true;
                        }
                    }

                    totalResult |= result;
                }

                return totalResult;
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
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="type"/>.</exception>
        /// <exception cref="ICSSoft.STORMNET.UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        public bool AccessObjectCheck(Type type, tTypeAccess operation, bool throwException)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var userRoles = GetUserRoleList();

            if (!(userRoles?.Any() ?? false))
                return false;

            AccessTypeAttribute accessTypeAttribute = GetAccessTypeAttribute(type);

            if (accessTypeAttribute == null || accessTypeAttribute.value == AccessType.none)
                return true;

            bool totalResult = false;

            foreach (var roleName in userRoles)
            {
                IEnumerable<Agent> roles = GetRolesByName(roleName);

                // Для каждой роли вычитаем дочерние роли и проверим наличие доступа.
                foreach (var role in roles)
                {
                    totalResult |= CheckTypeAccess(type, operation, role.AgentKey.Value);
                }
            }

            if (!totalResult && throwException)
                throw new UnauthorizedAccessException(operation.ToString(), type);

            return totalResult;
        }

        /// <summary>
        /// Проверка полномочий на выполнение операции с объектом.
        /// </summary>
        /// <param name="obj">Объект данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение в случае отсутствия прав.</param>
        /// <returns>Если у текущего пользователя есть доступ, то <c>true</c>.</returns>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="obj"/>.</exception>
        /// <exception cref="ICSSoft.STORMNET.UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        public bool AccessObjectCheck(object obj, tTypeAccess operation, bool throwException)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            // Проверяем полномочия на тип.
            Type type = obj.GetType();
            bool result = AccessObjectCheck(type, operation, throwException);

            return result;
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
        /// <param name="subjectType">Тип объекта.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="limit">Ограничение, которое есть для текущего пользователя.</param>
        /// <param name="canAccess">Есть ли доступ к этому типу у пользователя.</param>
        /// <returns>Результат выполнения операции.</returns>
        public OperationResult GetLimitForAccess(Type subjectType, tTypeAccess operation, out object limit, out bool canAccess)
        {
            canAccess = AccessObjectCheck(subjectType, operation, false);
            limit = null;

            return OperationResult.Успешно;
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
        /// Получение имени типа для системы полномочий в терминах физического хранения в СУБД.
        /// Данная реализация <see cref="ISecurityManager"/> поддерживает только режим, когда в БД хранятся значения <c>type.FullName</c>.
        /// </summary>
        /// <param name="type">Тип, для которого нужно получить имя.</param>
        /// <returns>Строковое имя типа, с которым будут работать внутренние механизмы системы полномочий.</returns>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="type"/>.</exception>
        protected virtual string GetSubjectTypeName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.FullName;
        }

        /// <summary>
        /// Метод для загрузки атрибута <see cref="AccessTypeAttribute"/> для типа.
        /// </summary>
        /// <param name="type">Тип, для которого нужно загрузить атрибут.</param>
        /// <returns>Экземпляр атрибута или <c>null</c>, если атрибут не был найден.</returns>
        private static AccessTypeAttribute GetAccessTypeAttribute(Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeof(AccessTypeAttribute), false);

            if (attrs.Length == 1)
                return (AccessTypeAttribute)attrs[0];

            if (attrs.Length == 0)
                return null;

            throw new InvalidOperationException($"Multiple [AccessTypeAttribute] detected for type {type.FullName}.");
        }

        /// <summary>
        /// Перечень ролей пользователя.
        /// </summary>
        private List<string> GetUserRoleList()
        {
            if (string.IsNullOrWhiteSpace(currentUser?.Roles))
                return null;

            return new List<string>(currentUser.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()));
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

        /// <summary>
        /// Получить список ролей из полномочий по имени роли.
        /// </summary>
        /// <param name="roleName">Имя роли.</param>
        /// <returns>Список ролей.</returns>
        private IEnumerable<Agent> GetRolesByName(string roleName)
        {
            var roles = dataService.Query<Agent>(Agent.Views.RoleOrGroupL)
                .Where(x => x.Name == roleName && x.IsRole)
                .Cast<Agent>();

            return roles;
        }

        /// <summary>
        /// Получить количество объектов доступа к операции.
        /// </summary>
        /// <param name="operationName">Имя опреации.</param>
        /// <param name="agentPKs">Список ключей агентов.</param>
        /// <returns>Количество объектов доступа.</returns>
        private int GetPermitionCount(string operationName, List<Guid> agentPKs)
        {
            var funcInParams = new object[agentPKs.Count + 1];
            funcInParams[0] = new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<Permition>(x => x.Agent));

            for (int i = 0; i < agentPKs.Count; i++)
            {
                funcInParams[i + 1] = agentPKs[i];
            }

            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Permition), Permition.Views.CheckAccessOperation);

            lcs.LimitFunction = langDef.GetFunction(
                langDef.funcAND,
                langDef.GetFunction(langDef.funcIN, funcInParams),
                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.BoolType, Information.ExtractPropertyPath<Permition>(x => x.Subject.IsOperation))),
                langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Permition>(x => x.Subject.Name)), operationName));

            // Количество ролей которым доступна указанная операция.
            var permitionsCount = dataService.GetObjectsCount(lcs);

            return permitionsCount;
        }

        /// <summary>
        /// Проверка полномочий на выполнение операции с типом.
        /// Реализует всю логику проверки доступа к типу, в т.ч. с учетом наследования (через <see cref="AccessType.@base"/>)  и <see cref="AccessType.this_and_base"/>.
        /// </summary>
        /// <param name="type">Тип объекта данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="agentKey">Ключ агента.</param>
        /// <returns>Если у текущего пользователя есть доступ, то <c>true</c>.</returns>
        private bool CheckTypeAccess(Type type, tTypeAccess operation, Guid agentKey)
        {
            AccessTypeAttribute accessTypeAttribute = GetAccessTypeAttribute(type);

            if (accessTypeAttribute == null)
                return true;

            if (accessTypeAttribute.value == AccessType.none)
                return true;

            bool result = true;
            if (accessTypeAttribute.value == AccessType.@base || accessTypeAttribute.value == AccessType.this_and_base)
                result = CheckTypeAccess(type.BaseType, operation, agentKey);

            if (!result)
                return false;

            string typeName = GetSubjectTypeName(type);

            try
            {
                Access[] acesses;
                string cacheKey = $"{nameof(CheckTypeAccess)}{agentKey}_{typeName}";

                if (cacheService != null && cacheService.Exists(cacheKey))
                {
                    acesses = cacheService.GetFromCache<Access[]>(cacheKey);
                }
                else
                {
                    var agentPKs = new List<Guid>();
                    AddLinkedAgents(agentKey, agentPKs);

                    object[] funcInParams = new object[agentPKs.Count + 1];
                    funcInParams[0] = new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<Access>(x => x.Permition.Agent));
                    for (int i = 0; i < agentPKs.Count; i++)
                        funcInParams[i + 1] = agentPKs[i];

                    Function lf = langDef.GetFunction(
                        langDef.funcAND,
                        langDef.GetFunction(langDef.funcIN, funcInParams),
                        langDef.GetFunction(langDef.funcLike, new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Access>(x => x.Permition.Subject.Name)), typeName));

                    LoadingCustomizationStruct lcsAccess = LoadingCustomizationStruct.GetSimpleStruct(typeof(Access), Access.Views.CheckAccessClass);
                    lcsAccess.LimitFunction = lf;

                    acesses = dataService.LoadObjects(lcsAccess).Cast<Access>().ToArray();
                    cacheService?.SetToCache(cacheKey, acesses, new string[] { SecurityCacheTags.SecurityCacheMainTag, SecurityCacheTags.SecurityManagerCacheTag });
                }

                return acesses.Any(ac => ac.TypeAccess == operation || ac.TypeAccess == tTypeAccess.Full);
            }
            catch (Exception exc)
            {
                LogService.LogError($"Error in checking access to type {typeName} with operation {operation}.", exc);
                throw;
            }
        }
    }
}
