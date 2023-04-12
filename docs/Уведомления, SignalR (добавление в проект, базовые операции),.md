# Инструкция по SignalR (добавление в проект и базовые функции)

## Что такое SignalR

SignalR Core это библиотека от компании Microsoft, которая работает поверх ASP.NET и которая предназначена для создания приложений, работающих в режиме реального времени. SignalR использует двунаправленную связь для обмена сообщениями между клиентом и сервером, благодаря чему сервер может отправлять клиентам в режиме реального времени некоторые данные и вызывать функции в коде клиента. Клиенты также могут обращаться к серверной части SignalR для вызова каких-то функций или общения с другими клиентами.

## Добавление серверной части (В бэкенде netcore 3.1)

1) Дополнительных пакетов устанавливать не нужно. Создадим класс-хаб, наследуемый от базового Hub SignalR. Hub - это сущность через которую происходит взаимодействие с клиентом
```
    namespace IIS.AsyncOpenXmlReportsSample
    {
        using System.Threading.Tasks;
        using Microsoft.AspNetCore.SignalR;
    
    	public class SignalRHub : Hub
        {
            public async Task SendNotifyUserMessage(string username)
            {
                await Clients.All.SendAsync("NotifyUser", $"Hi {username}");
            }
        }
    }
```

В данном примере описана только одна функция. Эту функцию можно вызывать из клиента. Выполняет вызов функции "NotifyUser" у всех активных клиентов. В параметре функции передает Hi {username}

2) Включим SignalR в *Startup.cs*

```
    public void ConfigureServices(IServiceCollection services)
    {
    	....
    	// Должно быть до включения CORS.
    	services.AddSignalR();
    	....
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    	// Стандартное описание корсов не подходит, надо так
    	app.UseCors(builder =>
    	{
    		builder.AllowAnyMethod()
    		.AllowAnyHeader()
    		.SetIsOriginAllowed(origin => true)
    		.AllowCredentials();
    	});
    
    	app.UseEndpoints(endpoints =>
    	{
    		....
    
    		// Добавляем роут к Hub.
    		endpoints.MapHub<SignalRHub>("/SignalRTest");
    	});
    }
```

## Добавление клиентской части (В ember 3)

1) Установим пакет signalR

```
    yarn add @aspnet/signalr
```

2) Добавим импорт в *ember-cli-build.js*

```
      app.import('node_modules/@aspnet/signalr/dist/browser/signalr.js');
```

3) Добавим *utils/signalr.js*. Класс для коннекта signalR

```
    import { resolve } from 'rsvp';
    
    class SignalRConnection {
      constructor(url) {
    
        // eslint-disable-next-line no-undef
        this.connection = new signalR.HubConnectionBuilder()
          .withUrl(url)
    
          // eslint-disable-next-line no-undef
          .configureLogging(signalR.LogLevel.Information)
          .build();
    
        this.connected = false;
      }
    
      start() {
        if (!this.connected && this.connection) {
          let self = this;
    
          return self.connection.start().then(function () {
            self.connected = true;
          });
        } else {
          return resolve();
        }
      }
    }
    
    export default SignalRConnection;
```

4) Добавим инициализатор соедиения initializers/signalr.js

```
    import SignalRConnection from '../utils/signalr';
    import config from '../config/environment';
    
    export function initialize(application) {
      let signalr = new SignalRConnection(config.APP.backendUrls.root + '/SignalRTest');
      application.register('realtime:signalr', signalr, { instantiate: false });
    }
```

5) Добавим миксин *mixins/signalr* для взаимодействия с SignalR

```
    import { getOwner } from '@ember/application';
    import Mixin from '@ember/object/mixin';
    
    export default Mixin.create({
      getSignalR() {
        const app = getOwner(this);
        const signalr = app.lookup('realtime:signalr');
    
        return signalr;
      },
      
      signalRConnect() {
        this.signalRTryToConnect();
      },
    
      signalRTryToConnect() {
        const _this = this;
        const signalr = _this.getSignalR();
    
        signalr.start().then(function () {
    
          // eslint-disable-next-line no-console
          console.log("SignalR Connected.");
    
          signalr.connection.on('NotifyUser', _this._notifyUser.bind(_this));
        }).catch(function (err) {
    
          // eslint-disable-next-line no-console
          console.log("SignalR NOT Connected." + err);
        });
      },
    
      _notifyUser(message) {
        // eslint-disable-next-line no-console
        console.log("SignalR notify User and send message - " + message);
      }
    });
```

В этом миксине мы также подписываем функцию *_notifyUser*   на вызов *NotifyUser* со стороны бэкенда.

6)  В controllers/application.js добавим вызов подключения к SignalR и тестовый экшен, который вызывает функцию на бэкенде в Hub SignalR.

```
    import Controller from '@ember/controller';
    ....
    import SignalRMixin from '../mixins/signalr';
    
    export default Controller.extend(SignalRMixin, {
    	....
      /**
        Initializes controller.
      */
      init() {
        this._super(...arguments);
    	....
        this.signalRConnect();
      },
    
      actions: {
        callSignarRTestNotification() {
          const signalR = this.getSignalR();
          if (signalR) {
            signalR.connection.invoke("SendNotifyUserMessage", this.userName);
          }
        },
      }
    });
```
