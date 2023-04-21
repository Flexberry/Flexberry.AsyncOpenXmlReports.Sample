# Формирование отчетов с использованием задач Quartz

## Схема взаимодействия

Есть [rest-сервис](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/Controllers/QuartzController.cs#L17), который принимает запросы на формирование отчетов. 

Каждый метод в сервисе отвечает за конкретный отчет и принимает свой набор параметров.

При поступлении запроса инициализируется задача для Quartz и отправляется в очередь на выполнение.

На каждый запрос сервиса существует свой тип задач для Quartz.

## Использование сервиса данных, полномочий и пр.

Регистрация сервисов описана в [настройках](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/AdapterStartup.cs#L74) хоста [в статье](/docs/%D0%A0%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D1%8F%20Host%2BWebHost%20%D0%B2%20%D0%BA%D0%BE%D0%BD%D1%81%D0%BE%D0%BB%D1%8C%D0%BD%D0%BE%D0%BC%20%D0%BF%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D0%B8.md). Все они регистрируются в едином контейнере **Unity.IUnityContainer**.

```C#
IDataService securityDataService = new PostgresDataService(new EmptySecurityManager()) { CustomizationString = securityConnStr };
IDataService mainDataService = new PostgresDataService() { CustomizationString = mainConnStr };

container.RegisterInstance(Configuration);
container.RegisterInstance<IDataService>("SecurityDataService", securityDataService, InstanceLifetime.Singleton);
container.RegisterInstance<IDataService>(mainDataService, InstanceLifetime.Singleton);

container.RegisterFactory<IUserWithRoles>((cont) => new AdapterUserService(), FactoryLifetime.PerThread);
container.RegisterFactory<ISecurityManager>(
    (cont) =>
    {
        var dataService = cont.Resolve<IDataService>("SecurityDataService");
        var cacheManager = cont.Resolve<ICacheService>();
        var userService = cont.Resolve<IUserWithRoles>();

        return new RoleSecurityManager(dataService, cacheManager, userService);
    },
    FactoryLifetime.PerThread);

container.RegisterSingleton<ICacheService, MemoryCacheService>();
```
- **securityDataService** - сервис данных для системы полномочий, чтобы она могла без ограничений вычитывать объекты полномочий для проверки. Регистрируется как **Singleton Instance**, т.е. он один на все приложение;
- **mainDataService** - главный сервис данных. Регистрируется как **Singleton Instance**, т.е. он один на все приложение;
- **IUserWithRoles** - сервис текущего пользователя. Регистрируется как **PerThread Factory**, т.е. будет создаваться новый объект на каждый поток в приложении, поэтому в параметрах указывается конструктор **new AdapterUserService()**.
- **ISecurityManager** - главный сервис полномочий. Регистрируется как **PerThread Factory**, т.е. будет создаваться новый объект на каждый поток в приложении, согласно указанному конструктору, в котором указывается севрис данных для полномочий, сервис кэша и сервис текущего пользователя.
- **ICacheService** - сервис кэша данных. Регистрируется как **Singleton Instance**, т.е. он один на все приложение.

### Пример получения сервисов для использования:
```C#
// Инициализация сервисов.
var ds = Adapter.Container.Resolve<IDataService>();
var user = Adapter.Container.Resolve<IUserWithRoles>();
var securityManager = Adapter.Container.Resolve<ISecurityManager>();
```

Вместо статического **Adapter.Container** можно использовать **ICSSoft.Services.UnityFactory.GetContainer()**.

### Пример использования сервиса полномочий
```C#
/// <summary>
/// Проверить доступность отчета в системе полномочий.
/// </summary>
/// <param name="operationName">Имя операции.</param>
/// <returns>Доступность отчета.</returns>
public bool AccessCheck(string operationName)
{
    var securityManager = Adapter.Container.Resolve<ISecurityManager>();

    return securityManager.AccessCheck(operationName);
}
```

## Пример реализации, отчет список машин

Метод сервиса [**CarListReport**](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/Controllers/QuartzController.cs#L54) 

```C#
var runTask = new Task(async () =>
{
    StdSchedulerFactory factory = new StdSchedulerFactory();
    IScheduler scheduler = await factory.GetScheduler();

    await scheduler.Start();

    var job = CarListJob.GetDetail("job1_" + request.Id, "group1_" + request.Id);
    var trigger = CarListJob.GetTrigger("trigger1_" + request.Id, "group1_" + request.Id);

    // Добавим к задаче данные запроса.
    job.JobDataMap.Add(JobTools.ReportNameParam, "CarListReport");
    job.JobDataMap.Add("CarListReportRequest", request);

    await scheduler.ScheduleJob(job, trigger);
});

runTask.Start();
```

Принимает данные в формате [**CarListReportRequest**](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/Controllers/RequestObjects/CarListReportRequest.cs#L8).

```C#
/// <summary>
/// Данные для запроса списка автомобилей.
/// </summary>
public class CarListReportRequest
{
    /// <summary>
    /// Идентификатор запроса.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Информация о пользователе.
    /// </summary>
    public UserInfo UserInfo { get; set; }

    /// <summary>
    /// Имя шаблона.
    /// </summary>
    public string TemplateName { get; set; }
}

/// <summary>
/// Информация о пользователе.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Домен пользователя.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FriendlyName { get; set; }

    /// <summary>
    /// Роли пользователя, разделенные запятоыми.
    /// </summary>
    public string Roles { get; set; }
}
```

Инициализирует задачу [**CarListJob**](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/Jobs/CarListJob.cs#L62), которая формирует отчет, записывает его в общую файловую систему.

```C#
var allCarsParameters =
    ds.Query<Car>(Car.Views.CarL)
        .ToList()
        .Select(car =>
            new Dictionary<string, object>()
                {
                    { "CarNumber", car.CarNumber },
                    { "CarDate", car.CarDate.ToString("dd.MM.yyyy") },
                    { "CarBody", EnumCaption.GetCaptionFor(car.CarBody) },
                })
        .ToList();

var fileName = JobTools.GetReportName(request.TemplateName, request.UserInfo.Login, request.Id);
var fullFileName = JobTools.GetFullReportName(fileName);
var parameters = new Dictionary<string, object> { { "Car", allCarsParameters } };
var fullTamplateName = JobTools.GetFullTemplateName(request.TemplateName);
var template = new DocxReport(fullTamplateName);

template.BuildWithParameters(parameters);
template.SaveAs(fullFileName);

return SendResultAsync(request.Id, fullFileName, "Executed");
```

И [вызывает метод бэекенда](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/01763d9c36401243b9120a15bebd33dd56bca27d/src/AsyncOpenXmlReportsSample/Quartz/Flexberry.Quartz.Sample.Service/Jobs/CarListJob.cs#L136), чтобы сообщить об успешном выполнении задачи.

```C#
/// <summary>
/// Отправить результат обработки файла.
/// </summary>
/// <param name="requestId">Идентификатор запроса.</param>
/// <param name="fullFileName">Имя файла отчета.</param>
/// <param name="status">Статус обработки. InProgress, Unexecuted, Executed.</param>
private static async Task SendResultAsync(string requestId, string fullFileName, string status)
{
    var sendResultUrl = JobTools.GetFullUrlPath("api/CarListReport", "BuildResult");

    object input = new
    {
        Id = requestId,
        FileName = fullFileName,
        Status = status,
    };
    var msg = JsonConvert.SerializeObject(input);

    LogService.Log.Debug($"CarListJob: Sending {msg} to {sendResultUrl}.");

    var buffer = Encoding.UTF8.GetBytes(msg);

    using (var byteContent = new ByteArrayContent(buffer))
    {
        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.PostAsync(sendResultUrl, byteContent).ConfigureAwait(true))
            {
                LogService.Log.Debug($"CarListJob: Sending status = {response.StatusCode}.");
            }
        }
    }
}
```