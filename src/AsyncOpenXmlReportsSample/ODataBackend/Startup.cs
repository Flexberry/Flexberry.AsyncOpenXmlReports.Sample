namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ICSSoft.Services;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations;
    using IIS.AsyncOpenXmlReportsSample.MailConfigurations.MailComponents;
    using IIS.AsyncOpenXmlReportsSample.Services;
    using IIS.AsyncOpenXmlReportsSample.Templates.MailTemplates;
    using IIS.Caseberry.Logging.Objects;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using NewPlatform.Flexberry.ORM.ODataService.Extensions;
    using NewPlatform.Flexberry.ORM.ODataService.Files;
    using NewPlatform.Flexberry.ORM.ODataService.Model;
    using NewPlatform.Flexberry.ORM.ODataService.WebApi.Extensions;
    using NewPlatform.Flexberry.ORM.ODataServiceCore.Common.Exceptions;
    using NewPlatform.Flexberry.Services;
    using Unity;

    /// <summary>
    /// Класс настройки запуска приложения.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">An application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// An application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configurate application services.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        /// <param name="services">An collection of application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connStr = Configuration["DefConnStr"];

            services.AddMvcCore(
                    options =>
                    {
                        options.Filters.Add<CustomExceptionFilter>();
                        options.EnableEndpointRouting = false;
                    })
                .AddFormatterMappings();

            services.AddOData();

            services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                options.Authority = "http://localhost:8080/realms/master/";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
                options.RequireHttpsMetadata = false;
              });

            services.AddAuthorization();

            services.AddControllers().AddControllersAsServices();

            services.AddCors();
            services
                .AddHealthChecks()
                .AddNpgSql(connStr);

            services.Configure<MailConfigurations.EmailOptions>(this.Configuration.GetSection("Email"));
        }

        /// <summary>
        /// Configurate the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
        /// <param name="app">An application configurator.</param>
        /// <param name="env">Information about web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            LogService.LogInfo("Инициирован запуск приложения.");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseODataService(builder =>
            {
                builder.MapFileRoute();

                var assemblies = new[]
                {
                    typeof(ObjectsMarker).Assembly,
                    typeof(ApplicationLog).Assembly,
                    typeof(UserSetting).Assembly,
                    typeof(Lock).Assembly,
                };
                var modelBuilder = new DefaultDataObjectEdmModelBuilder(assemblies, true);

                var token = builder.MapDataObjectRoute(modelBuilder);
            });
        }

        /// <summary>
        /// Configurate application container.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        public void ConfigureContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // FYI: сервисы, в т.ч. контроллеры, создаются из дочернего контейнера.
            while (container.Parent != null)
            {
                container = container.Parent;
            }

            // FYI: сервис данных ходит в контейнер UnityFactory.
            container.RegisterInstance(Configuration);

            container.RegisterType<IHttpContextAccessor, HttpContextAccessor>();

            // Регистрируем CurrentUserService.
            ICSSoft.Services.CurrentUserService.IUser userServise = new CurrentHttpUserService(container.Resolve<IHttpContextAccessor>());
            container.RegisterInstance<ICSSoft.Services.CurrentUserService.IUser>(userServise, InstanceLifetime.Singleton);

            RegisterDataObjectFileAccessor(container);
            RegisterORM(container);

            var emailOptions = new MailConfigurations.EmailOptions();
            Configuration.GetSection("Email").Bind(emailOptions);
            container.RegisterInstance(emailOptions);
            container.RegisterType<Services.IEmailSender, Services.MailKitEmailService>();
            SendStartUpLetter();
        }

        /// <summary>
        /// Register implementation of <see cref="IDataObjectFileAccessor"/>.
        /// </summary>
        /// <param name="container">Container to register at.</param>
        private void RegisterDataObjectFileAccessor(IUnityContainer container)
        {
            const string fileControllerPath = "api/file";
            string baseUriRaw = Configuration["BackendRoot"];
            if (string.IsNullOrEmpty(baseUriRaw))
            {
                throw new System.Configuration.ConfigurationErrorsException("BackendRoot is not specified in Configuration or enviromnent variables.");
            }

            Console.WriteLine($"baseUriRaw is {baseUriRaw}");
            var baseUri = new Uri(baseUriRaw);
            string uploadPath = Configuration["UploadUrl"];
            container.RegisterSingleton<IDataObjectFileAccessor, DefaultDataObjectFileAccessor>(
                Invoke.Constructor(
                    baseUri,
                    fileControllerPath,
                    uploadPath,
                    null));
        }

        /// <summary>
        /// Register ORM implementations.
        /// </summary>
        /// <param name="container">Container to register at.</param>
        private void RegisterORM(IUnityContainer container)
        {
            string connStr = Configuration["DefConnStr"];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new System.Configuration.ConfigurationErrorsException("DefConnStr is not specified in Configuration or enviromnent variables.");
            }

            ISecurityManager emptySecurityManager = new EmptySecurityManager();
            string securityConnectionString = connStr;
            IDataService securityDataService = new PostgresDataService(emptySecurityManager)
            {
                CustomizationString = securityConnectionString
            };

            IHttpContextAccessor contextAccesor = new HttpContextAccessor();
            container.RegisterInstance<IHttpContextAccessor>(contextAccesor);
            string mainConnectionString = connStr;
            IDataService mainDataService = new PostgresDataService()
            {
                CustomizationString = mainConnectionString
            };

            container.RegisterInstance<IDataService>(mainDataService, InstanceLifetime.Singleton);

            container.RegisterSingleton<ISecurityManager, EmptySecurityManager>();
            container.RegisterSingleton<IDataService, PostgresDataService>(
                Inject.Property(nameof(PostgresDataService.CustomizationString), connStr));
        }

        /// <summary>
        /// Отправить письмо о запуске почтового сервиса.
        /// </summary>
        public static void SendStartUpLetter()
        {
            var subject = "Запуск почтового сервиса";
            string message = $"Настоящим письмом сообщаем об успешном запуске почтового сервиса. " +
                            $"Настоятельно рекомендуем проверить наличие прикреплённого файла." +
                            $"<br><br>" +
                            $"Мы рады, что у Вас всё получилось!<br>" +
                            $"<br><br>" +
                            $"С уважением,<br>" +
                            $"Служба поддержки сервиса рассылки отчётов.";
            T4MailTemplate mailCommonRu = new T4MailTemplate()
            {
                Subject = subject,
                HtmlMessage = message,
                Greetings = $"Доброго времени суток, мой юный друг!",
            };
            string messageBody = mailCommonRu.TransformText();

            MailDataBase mailData = new MailDataBase()
            {
                From = new Contact("sender", "mailsender@neoplatform.ru"),
                To = new List<Contact>()
                {
                    new Contact("recipient 1", "recipient1@neoplatform.ru"),
                },
                CopyTo = new List<Contact>()
                {
                    new Contact("recipient to copy", "recipientcopy@neoplatform.ru"),
                },
                Subject = subject,
                Body = messageBody,
            };

            string s = "Раз содержимое этого файла предстало перед Вашим взором, " +
                "то мы поздравляем с очередным успехом!\n" +
                "Верьте в себя и у Вас всё обязательно получится!";
            byte[] bitString = Encoding.UTF8.GetBytes(s);
            AttachmentFile attachmentFile = new AttachmentFile()
            {
                Name = "attachment.txt",
                Body = bitString,
            };

            try
            {
                var container = UnityFactory.GetContainer();
                var emailService = container.Resolve<IEmailSender>();
                emailService.SendEmail(mailData, attachmentFile);
            }
            catch (Exception ex)
            {
                LogService.LogError($"SendMail ERROR", ex);
            }
        }
    }
}
