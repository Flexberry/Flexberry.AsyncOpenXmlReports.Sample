using System.Threading.Tasks;

namespace IIS.AsyncOpenXmlReportsSample.Services
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}
