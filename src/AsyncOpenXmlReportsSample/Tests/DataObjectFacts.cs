/*
 * В этом файле можно настраивать поведение тестов.
 */
namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Класс с тестами для проверки корректности объектной модели.
    /// </summary>
    public partial class DataObjectFacts
    {
        /// <summary>
        /// Проверять ли, что во всех объектах данных AcessType=@this.
        /// </summary>
        private bool CheckAccessTypeThis { get; } = false;

        /// <summary>
        /// Проверять ли, что во всех объектах настроен аудит.
        /// </summary>
        private bool CheckAudit { get; } = false;

        /// <summary>
        /// Проверять ли, что во всех объектах данных есть атрибут Serializable.
        /// </summary>
        private bool CheckSerializable { get; } = false;


        /// <summary>
        /// Получить используемый приложением тип сервиса данных.
        /// </summary>
        /// <returns>Тип сервиса данных.</returns>
        private partial Type GetDataServiceType()
        {
            return typeof(PostgresDataService);
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// для которых намеренно не задано DataServiceExpression.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых не задано DataServiceExpression.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutDataServiceExpression()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных с ValueType-типом,
        /// для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutNotNull()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых геттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых геттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutGetterCheck()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых сеттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых сеттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertiesWithoutSetterCheck()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена представлений объектов данных, являющихся детейлами,
        /// в которых намеренно отсутствует свойство-агрегатор.
        /// </summary>
        /// <returns><see cref="Dictionary{Type, string[]}"/> с именами представлений объектов-детейлов,
        /// в которых намеренно отсутсвует свойство-агрегатор.</returns>
        private partial Dictionary<Type, string[]> GetViewsWithoutAgregateProperty()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить классы объектов данных, 
        /// в которых имеются намеренно некорректные представления.
        /// </summary>
        /// <returns>Список классов с намеренно некорректными представлениями.</returns>
        private partial IEnumerable<Type> GetTypesWithInvalidViews()
        {
            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не задан AcessType=@this.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не задан AcessType=@this.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAccessType()
        {
            if (CheckAccessTypeThis)
            {
                return Enumerable.Empty<Type>();
            }
            else
            {
                return GetStoredDataObjects();
            }
        }

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не задан Serializable.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не задан Serializable.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutSerializable()
        {
            if (CheckSerializable)
            {
                return Enumerable.Empty<Type>();
            }
            else
            {
                return GetStoredDataObjectsAndDetails();
            }
        }

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не настроен аудит.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не настроен аудит.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAudit()
        {
            if (CheckAudit)
            {
                return Enumerable.Empty<Type>();
            }
            else
            {
                return GetStoredDataObjects();
            }
        }

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых намеренно не настроен аудит операций.
        /// </summary>
        /// <returns>Список классов, в которых намеренно не настроен аудит операций.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAuditOperations()
        {
            if (CheckAudit)
            {
                return Enumerable.Empty<Type>();
            }
            else
            {
                return GetStoredDataObjects();
            }
        }
    }
}
