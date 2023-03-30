/*
 * Этот файл генерируется каждый раз при генерации проекта объектов.
 * Не нужно вносить изменения в этот файл вручную.
 * Настроить поведение тестов можно в соседнем файле,
 * который генерируется только при первой генерации.
 */

namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using Xunit;

    public partial class DataObjectFacts
    {
        #region Customizations

        /// <summary>
        /// Получить используемый приложением тип сервиса данных.
        /// </summary>
        /// <returns>Тип сервиса данных.</returns>
        private partial Type GetDataServiceType();

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// для которых намеренно не задано DataServiceExpression.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых не задано DataServiceExpression.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutDataServiceExpression();

        /// <summary>
        /// Получить имена свойств объектов данных с ValueType-типом,
        /// для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutNotNull();

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых геттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых геттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutGetterCheck();

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых сеттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых сеттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutSetterCheck();

        /// <summary>
        /// Получить имена представлений объектов данных, являющихся детейлами,
        /// в которых намеренно отсутствует свойство-агрегатор.
        /// </summary>
        /// <returns><see cref="Dictionary{Type, string[]}"/> с именами представлений объектов-детейлов,
        /// в которых намеренно отсутсвует свойство-агрегатор.</returns>
        private partial Dictionary<Type, string[]> GetViewsWithoutAgregateProperty();

        /// <summary>
        /// Получить классы объектов данных, 
        /// в которых имеются намеренно некорректные представления.
        /// </summary>
        /// <returns>Список классов с намеренно некорректными представлениями.</returns>
        private partial IEnumerable<Type> GetTypesWithInvalidViews();

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не задан AcessType=@this.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не задан AcessType=@this.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAccessType();

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не задан Serializable.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не задан Serializable.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutSerializable();

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не настроен аудит.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не настроен аудит.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAudit();

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не настроен аудит операций.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не настроен аудит операций.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAuditOperations();
        #endregion

        /// <summary>
        /// Получить хранимые объекты данных из сборки объектов данных.
        /// </summary>
        /// <returns>Список классов хранимых объектов данных из сборки объектов данных.</returns>
        private IEnumerable<Type> GetStoredDataObjects()
        {
            return Assembly.GetAssembly(typeof(ObjectsMarker))
                .GetExportedTypes()
                .Where(
                    x => x.IsClass
                         && x.IsSubclassOf(typeof(DataObject))
                         && Information.IsStoredType(x))
                .OrderBy(x => x.FullName);
        }

        /// <summary>
        /// Получить хранимые объекты данных и массивы детейлов из сборки объектов данных.
        /// </summary>
        /// <returns>Список классов хранимых объектов данных и массивов детейлов из сборки объектов данных.</returns>
        private IEnumerable<Type> GetStoredDataObjectsAndDetails()
        {
            return Assembly.GetAssembly(typeof(ObjectsMarker))
                .GetExportedTypes()
                .Where(
                    x => x.IsClass
                         && ((x.IsSubclassOf(typeof(DataObject)) && Information.IsStoredType(x))
                            || x.IsSubclassOf(typeof(DetailArray))))
                .OrderBy(x => x.FullName);
        }

        private static IEnumerable<string> GetPropertyNames<T>()
        {
            return typeof(T).GetProperties().Select(dataObjProp => dataObjProp.Name).ToArray();
        }

        /// <summary>
        /// Получить свойства, определенные в интерфейсе <see cref="IDataObjectWithAuditFields" />.
        /// </summary>
        /// <returns>Перечень свойств.</returns>
        private static IEnumerable<string> AuditProperties()
        {
            return GetPropertyNames<IDataObjectWithAuditFields>();
        }

        /// <summary>
        /// Получить свойства, определенные в интерфейсе <see cref="DataObject" />.
        /// </summary>
        /// <returns>Перечень свойств.</returns>
        private static IEnumerable<string> DataObjectInnerProperties()
        {
            return GetPropertyNames<DataObject>();
        }

        /// <summary>
        ///     Тест проверяет, что все нехранимые свойства имеют DSE.
        /// </summary>
        [Fact]
        public void TestAllNotStoredPropertiesHaveDataServiceExpression()
        {
            // Arrange.
            var dataServiceType = GetDataServiceType();
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertiesWithoutDataServiceExpression();
            var dontCheckPropertyNames = typeof(DataObject).GetProperties()
                .Where(o => !Information.IsStoredProperty(typeof(DataObject), o.Name))
                .Select(o => o.Name);

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var classProperties = cl.GetProperties()
                    .Where(// Не проверяем исключения на имя свойства.
                        o => !dontCheckPropertyNames.Contains(o.Name))
                    .Where(// Не проверяем пары имя свойства + класс.
                        o => !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(o.Name)))
                    .Where(
                        o => !Information.IsStoredProperty(cl, o.Name)
                             && string.IsNullOrEmpty(Information.GetExpressionForProperty(cl, o.Name).GetMostCompatible(dataServiceType)?.ToString())
                             && !o.PropertyType.IsSubclassOf(typeof(DataObject)));
                errors.AddRange(classProperties.Select(prop => $"{cl.FullName}.{prop.Name}"));
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие нехранимые свойства классов не имеют DSE:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что все логические поля имеют атрибут NotNull.
        /// </summary>
        [Fact]
        public void TestAllValueTypeHasNotNullFlag()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertiesWithoutNotNull();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var classProperties = cl.GetProperties();
                foreach (var property in classProperties)
                {
                    var propType = Information.GetPropertyType(cl, property.Name);
                    if (!propType.IsConstructedGenericType
                        && !propType.IsEnum
                        && propType.IsValueType
                        && Information.IsStoredProperty(cl, property.Name)
                        && !Information.GetPropertyNotNull(cl, property.Name)
                        && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(property.Name)))
                    {
                        errors.Add($@"{cl.FullName}.{property.Name}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие ValueType свойства классов не имеют флага NotNull:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что getter'ы всех свойств не вызывают исключений при пустом объекте.
        /// </summary>
        [Fact]
        public void TestAllGettersAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertiesWithoutGetterCheck();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var instance = Activator.CreateInstance(cl);
                var classProperties = cl.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetMethod != null
                                && x.DeclaringType != typeof(DataObject)
                                && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(x.Name)))
                    .ToList();
                foreach (var prop in classProperties)
                {
                    try
                    {
                        prop.GetValue(instance, null);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($@"{cl.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие у следующих свойств getter'ы некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что setter'ы всех свойств не вызывают исключений при пустом объекте.
        /// </summary>
        [Fact]
        public void TestAllSettersAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertiesWithoutSetterCheck();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var instance = Activator.CreateInstance(cl);
                var classProperties = cl.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.SetMethod != null
                                && x.DeclaringType != typeof(DataObject)
                                && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(x.Name)))
                    .ToList();
                foreach (var prop in classProperties)
                {
                    var val = prop.PropertyType.IsValueType
                        ? Activator.CreateInstance(prop.PropertyType)
                        : null;
                    try
                    {
                        prop.SetValue(instance, val);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($@"{cl.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие у следующих свойств setter'ы некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что атрибут <see cref="BusinessServerAttribute" /> корректен.
        /// </summary>
        [Fact]
        public void TestAllBusinessServerNamesAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();

            // Act.
            var errors = new List<string>();
            foreach (var type in assemblyClasses)
            {
                try
                {
                    var atrs = type.GetCustomAttributes<BusinessServerAttribute>(false).ToList();
                    if (atrs.Count > 1)
                    {
                        errors.Add($"{type.FullName}: более одного атрибута {nameof(BusinessServerAttribute)}.");
                    }
                    else if (atrs.Any())
                    {
                        var atr = atrs.First();
                        var method = atr.BusinessServerType.GetMethod(
                            $"OnUpdate{type.Name}",
                            BindingFlags.Public | BindingFlags.Instance);
                        if (method == null)
                        {
                            errors.Add($"{type.FullName}: не найден метод \"OnUpdate{type.Name}\" в БСе {atr.BusinessServerType.FullName}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($@"{type.FullName}: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих классов атрибут {nameof(BusinessServerAttribute)} некорректен:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что у всех хранимых классов DataObject-свойства имеют <see cref="PropertyStorageAttribute"/>.
        /// </summary>
        [Fact]
        public void TestAllDataObjectPropertyHasPropertyStorageAttribute()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                var classProperties = type.GetProperties()
                    .Where(
                        prop => Information.IsStoredProperty(type, prop.Name)
                             && prop.PropertyType.IsSubclassOf(typeof(DataObject))
                             && !prop.GetCustomAttributes<PropertyStorageAttribute>().Any());
                errors.AddRange(classProperties.Select(prop => $"{type.FullName}.{prop.Name}"));
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих хранимых классов DataObject-свойства отсутствует атрибут [PropertyStorage]:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что статические представления во вложенном классе Views валидны.
        /// </summary>
        [Fact]
        public void TestStaticViewsAreValid()
        {
            // Arrange.
            var dontCheckClasses = GetTypesWithInvalidViews();
            var assemblyClasses = GetStoredDataObjects();
            var checkedTypes = assemblyClasses.Where(x => !dontCheckClasses.Contains(x)).ToList();

            // Act.
            var errors = new List<string>();
            foreach (var type in checkedTypes)
            {
                var viewsType = type.GetNestedType("Views");
                if (viewsType == null)
                {
                    errors.Add($@"{type.FullName}: не содержит Views-subclass.");
                }
                else
                {
                    var properties = viewsType.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.GetValue(null, null);
                        }
                        catch (Exception ex)
                        {
                            errors.Add($@"{type.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                        }
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих классов Views-subclass некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что во всех представлениях детейлов присутствует свойство агрегатора.
        /// </summary>
        [Fact]
        public void TestDetailViews()
        {
            // Arrange.
            var viewsWithoutAgregateProperty = GetViewsWithoutAgregateProperty();
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                string agrProp = Information.GetAgregatePropertyName(type);
                if (string.IsNullOrEmpty(agrProp))
                    continue;

                var viewNames = Information.AllViews(type);
                foreach (string viewName in viewNames.Where(x => !viewsWithoutAgregateProperty.ContainsKey(type)
                                                                || !viewsWithoutAgregateProperty[type].Contains(x)))
                {
                    var view = Information.GetView(viewName, type);
                    if (view.Properties.All(p => p.Name != agrProp))
                    {
                        errors.Add($"{type.FullName} {viewName}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}В следующих представлениях детейловых классов не найдены свойство агрегатора:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что для всех нехранимых свойств добавлены необходимые зависимые свойства.
        /// </summary>
        [Fact]
        public void TestNotStoredPropertiesInViews()
        {
            // Arrange.
            var dataServiceType = GetDataServiceType();
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                var viewNames = Information.AllViews(type);
                foreach (string viewName in viewNames)
                {
                    // Эталонное представление.
                    var view = Information.GetView(viewName, type);

                    // Представление, в которое будут добавлены зависимые свойства.
                    var viewAppended = Information.GetView(viewName, type);
                    Information.AppendPropertiesFromNotStored(viewAppended, dataServiceType);

                    // Выделим свойства, которые были добавлены методом Information.AppendPropertiesFromNotStored.
                    var extraProps = viewAppended.Properties.Where(p => !view.CheckPropname(p.Name)).ToList();
                    if (extraProps.Any())
                    {
                        errors.Add($"{type.FullName} {viewName}:{Environment.NewLine}{string.Join(Environment.NewLine, extraProps.Select(p => p.Name))}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}В следующих представлениях обнаружены ошибки нехранимых свойств:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что у всех классов присутствует атрибут <see cref="SerializableAttribute" />.
        /// </summary>
        [Fact]
        public void TestAllDataObjectHasSerializableAttribute()
        {
            // Arrange.
            var dontCheckClasses = GetDataObjectsWithoutSerializable().ToList();
            var types = Assembly.GetAssembly(typeof(ObjectsMarker)).GetExportedTypes();

            // Act.
            var notSerializableTypes = types
                .Where(
                    t => !dontCheckClasses.Contains(t))
                .Where(
                x => x.IsClass
                     && (x.IsSubclassOf(typeof(DataObject)) || x.IsSubclassOf(typeof(DetailArray)))
                     && !x.IsSerializable)
                .Select(x => x.FullName)
                .ToList();

            // Assert.
            Assert.False(
                notSerializableTypes.Any(),
                $"{Environment.NewLine}У классов отсутствует атрибут [Serializable]:{Environment.NewLine}{string.Join(Environment.NewLine, notSerializableTypes)}");
        }

        /// <summary>
        ///     Тест проверяет, что все объекты данных имеют <see cref="AccessType.@this" />.
        /// </summary>
        [Fact]
        public void TestAllDataObjectsHasAccessTypeThis()
        {
            // Arrange.
            var dontCheckClasses = GetDataObjectsWithoutAccessType().ToList();

            // Act.
            var storedDataObjects = GetStoredDataObjects()
                .Where( // и он не содержится в списке исключений.
                    t => !dontCheckClasses.Contains(t))
                .Where( // он имеет другой аксесс тайп.
                    t =>
                        Attribute.GetCustomAttribute(t, typeof(AccessTypeAttribute)) == null
                        || ((AccessTypeAttribute)Attribute.GetCustomAttribute(t, typeof(AccessTypeAttribute))).value != AccessType.@this)
                .ToList();

            // Assert.
            Assert.False(
                storedDataObjects.Any(),
                $"{typeof(AccessType).Name} отличается от {nameof(AccessType.@this)} в следующих классах:{Environment.NewLine}" + string.Join(Environment.NewLine, storedDataObjects.Select(x => x.FullName)));
        }

        /// <summary>
        /// Проверка наличия настроек аудита для всех классов и их полей.
        /// </summary>
        [Fact]
        public void TestExistenceAuditSettings()
        {
            var errorMessages = new List<string>();

            var exceptions = GetDataObjectsWithoutAudit();

            var assemblyClasses = GetStoredDataObjects().Except(exceptions);

            foreach (var curClass in assemblyClasses)
            {
                var auditSettings = curClass.GetNestedType(AuditConstants.AuditSettingsClassName);
                if (auditSettings == null)
                {
                    errorMessages.Add($"{curClass.FullName}: Не найден подкласс для настроек аудита.");
                }
                else
                {
                    bool auditEnabled = (bool)auditSettings.GetField(AuditConstants.AuditEnabledFieldName).GetValue(null);
                    if (!auditEnabled)
                    {
                        errorMessages.Add($"{curClass.FullName}: Не включен аудит.");
                    }
                }
            }

            Assert.False(
                errorMessages.Any(),
                string.Join(Environment.NewLine, errorMessages));
        }

        /// <summary>
        /// Проверка наличия настроек аудита для всех классов и их полей.
        /// </summary>
        [Fact]
        public void TestAuditOperationsAreValid()
        {
            var errorMessages = new List<string>();

            var exceptionsGlobal = GetDataObjectsWithoutAudit();
            var exceptions = GetDataObjectsWithoutAuditOperations();

            var assemblyClasses = GetStoredDataObjects().Except(exceptionsGlobal).Except(exceptions);

            foreach (var curClass in assemblyClasses)
            {
                var auditSettings = curClass.GetNestedType(AuditConstants.AuditSettingsClassName);
                if (auditSettings == null)
                {
                    errorMessages.Add($"{curClass.FullName}: Не найден подкласс для настроек аудита.");
                }
                else
                {
                    bool insertAuditEnable = (bool)auditSettings.GetField(AuditConstants.InsertAuditFieldName).GetValue(null);
                    if (!insertAuditEnable)
                    {
                        errorMessages.Add($"{curClass.FullName}: Не включен аудит операции создания.");
                    }

                    bool updateAuditEnable = (bool)auditSettings.GetField(AuditConstants.UpdateAuditFieldName).GetValue(null);
                    if (!updateAuditEnable)
                    {
                        errorMessages.Add($"{curClass.FullName}: Не включен аудит операции изменения.");
                    }

                    bool deleteAuditEnable = (bool)auditSettings.GetField(AuditConstants.DeleteAuditFieldName).GetValue(null);
                    if (!deleteAuditEnable)
                    {
                        errorMessages.Add($"{curClass.FullName}: Не включен аудит операции удаления.");
                    }
                }
            }

            Assert.False(
                errorMessages.Any(),
                string.Join(Environment.NewLine, errorMessages));
        }

        /// <summary>
        /// Проверка наличия собственных свойств в представлениях аудита.
        /// </summary>
        [Fact]
        public void TestOwnPropertiesInAuditView()
        {
            var errorMessages = new List<string>();

            var exceptions = GetDataObjectsWithoutAudit();

            var assemblyClasses = GetStoredDataObjects().Except(exceptions);

            /* Проверяем наличие подкласса аудита в классах-наследниках DataObject,
               проверяем наличие всех свойств класса (с возможными исключениями) в представлении аудита. */
            var ignorePropertyNames = AuditProperties().Union(DataObjectInnerProperties());
            foreach (var curClass in assemblyClasses)
            {
                var classProperties = curClass.GetProperties()
                    .Where(x => Information.IsStoredProperty(curClass, x.Name) && !ignorePropertyNames.Contains(x.Name))
                    .Select(property => property.Name)
                    .ToList();

                var auditView = Information.GetView(AuditConstants.DefaultAuditViewName, curClass);
                if (auditView == null)
                {
                    errorMessages.Add($"{curClass.FullName}: Не найдено представление аудита AuditView.");
                }
                else
                {
                    var viewProperties = auditView.Properties
                        .Where(x => x.Name.IndexOf('.') == -1)
                        .Select(x => x.Name)
                        .ToList();
                    viewProperties.AddRange(auditView.Details.Select(detail => detail.Name));

                    var propertiesWithNoSettings = classProperties.Where(x => !viewProperties.Contains(x));
                    errorMessages.AddRange(
                        propertiesWithNoSettings
                            .Select(x => $"{curClass.FullName}: Не найдено свойство {x} в отображении аудита."));
                }
            }

            Assert.False(
                errorMessages.Any(),
                string.Join(Environment.NewLine, errorMessages));
        }

        /// <summary>
        /// Проверка валидности представления аудита.
        /// </summary>
        [Fact]
        public void TestAllAuditViewsAreValid()
        {
            var errorMessages = new List<string>();

            var exceptions = GetDataObjectsWithoutAudit();

            var assemblyClasses = GetStoredDataObjects().Except(exceptions);

            foreach (var curClass in assemblyClasses)
            {
                var auditView = Information.GetView(AuditConstants.DefaultAuditViewName, curClass);
                if (auditView == null)
                {
                    errorMessages.Add($"{curClass.FullName}: Не найдено представление аудита AuditView.");
                }
                else
                {
                    var viewProperties = auditView.Properties
                        .Select(x => x.Name)
                        .ToList();

                    errorMessages.AddRange(
                        viewProperties
                            .Where(x => !Information.IsStoredProperty(curClass, x))
                            .Select(x => $"{curClass.FullName}: Содержит нехранимое свойство {x} в AuditView."));

                    var viewDetails = auditView.Details;

                    errorMessages.AddRange(
                        viewDetails
                            .Where(x => x.View.Name != AuditConstants.DefaultAuditViewName)
                            .Select(x => $"{curClass.FullName}: Содержит детейл {x.Name} с именем отличным от AuditView ({x.View.Name})."));
                }
            }

            Assert.False(
               errorMessages.Any(),
               string.Join(Environment.NewLine, errorMessages));
        }
    }
}
