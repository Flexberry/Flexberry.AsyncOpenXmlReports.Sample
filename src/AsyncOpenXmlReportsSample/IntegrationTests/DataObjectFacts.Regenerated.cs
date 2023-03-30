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
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;

    public partial class DataObjectFacts
    {
        private readonly IDataService _dataService;

        public DataObjectFacts(IDataService dataService) => _dataService = dataService;

        #region Customizations

        /// <summary>
        /// Получить классы объектов данных,
        /// которые не требуется вычитывать из БД.
        /// </summary>
        /// <returns>Список классов, которые не требуется вычитывать из БД.</returns>
        private partial IEnumerable<Type> GetNotSelectableTypes();
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
        ///     Проверить, что все хранимые классы могут читаться по всем представлениям.
        /// </summary>
        [Fact]
        public void CheckAllStoredClassesAreSelectable()
        {
            // Arrange.
            var dontCheckClasses = GetNotSelectableTypes();
            var storedTypes = GetStoredDataObjects().Except(dontCheckClasses).ToList();

            // Act.
            var errors = new Dictionary<string, string>();
            foreach (var type in storedTypes)
            {
                var viewNames = Information.AllViews(type);
                var viewOnlyThat = new View(type, View.ReadType.OnlyThatObject);
                var views = viewNames.Select(x => Information.GetView(x, type)).Union(new[] { viewOnlyThat });
                foreach (var view in views)
                {
                    try
                    {
                        var lcs = LoadingCustomizationStruct.GetSimpleStruct(type, view);
                        lcs.ReturnTop = 1;
                        _dataService.LoadObjects(lcs);
                    }
                    catch (Exception ex)
                    {
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }

                        string key = $"{type.FullName}\t{view.Name}";
                        errors[key] = ex.Message;
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                string.Join(Environment.NewLine, errors.OrderBy(x => x.Key).Select(x => $"{x.Key}:{Environment.NewLine}{x.Value}")));
        }

        /// <summary>
        ///     Проверить, что все хранимые классы имеют корректную интеграцию по полям Enum.
        /// </summary>
        [Fact]
        public void CheckAllEnumIntegration()
        {
            // Arrange.
            var storedTypes = GetStoredDataObjects().ToList();
            var storedTypesWithEnum = storedTypes.Where(x => x.GetProperties().Any(p => p.PropertyType.IsEnum));

            // Act.
            var errors = new Dictionary<string, string>();
            foreach (var type in storedTypesWithEnum)
            {
                var enumProps = type.GetProperties().Where(p => p.PropertyType.IsEnum && Information.IsStoredProperty(type, p.Name));
                foreach (var prop in enumProps)
                {
                    var view = new View { DefineClassType = type };
                    view.AddProperty(prop.Name);

                    var enumCaptions = Enum.GetValues(prop.PropertyType).Cast<object>().Select(EnumCaption.GetCaptionFor).ToList();

                    try
                    {
                        // Вычитаем из БД все строки, у которых `prop.Name` не пусто и не из списка доступных `Caption`.
                        var varDef = new VariableDef(ExternalLangDef.LanguageDef.StringType, prop.Name);
                        var lcs = LoadingCustomizationStruct.GetSimpleStruct(type, view);
                        lcs.LimitFunction = FunctionBuilder.BuildAnd(
                            FunctionBuilder.BuildIsNotNull(prop.Name),
                            FunctionBuilder.BuildNot(
                                FunctionBuilder.BuildOr(enumCaptions.Select(x => FunctionBuilder.BuildEquals(varDef, x)))));
                        int failedObjects = _dataService.GetObjectsCount(lcs);

                        // Можно посчитать только кол-во, ибо сами объекты не получится загрузить (ошибка парсинга Enum).
                        // Можно попробовать вытащить сырые значения через LoadStringedObjectView, но проще от этого не станет.
                        if (failedObjects > 0)
                        {
                            string key = $"{type.FullName}.{prop.Name}\t{prop.PropertyType}";
                            string sql = (_dataService as SQLDataService)?.GenerateSQLSelect(lcs, false) ?? string.Empty;
                            errors[key] = $"Обнаружено {failedObjects} объектов не соответствующих {prop.PropertyType}:{Environment.NewLine}{sql}";
                        }
                    }
                    catch (Exception ex)
                    {
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }

                        string key = $"{type.FullName}.{prop.Name}\t{prop.PropertyType}";
                        errors[key] = ex.Message;
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                string.Join(Environment.NewLine, errors.OrderBy(x => x.Key).Select(x => $"{x.Key}:{Environment.NewLine}{x.Value}")));
        }
    }
}
