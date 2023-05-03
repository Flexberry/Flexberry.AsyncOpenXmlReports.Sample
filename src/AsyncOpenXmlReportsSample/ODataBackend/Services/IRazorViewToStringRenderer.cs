namespace IIS.AsyncOpenXmlReportsSample.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Интефейс рендеринга представлений с синтаксисом Razor.
    /// </summary>
    public interface IRazorViewToStringRenderer
    {
        /// <summary>
        /// Рендеринг представлений с синтаксисом Razor.
        /// </summary>
        /// <typeparam name="TModel">Тип модели представления.</typeparam>
        /// <param name="viewName">Представление.</param>
        /// <param name="model">Модель представления.</param>
        /// <returns>Отрендеринное представление.</returns>
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}
