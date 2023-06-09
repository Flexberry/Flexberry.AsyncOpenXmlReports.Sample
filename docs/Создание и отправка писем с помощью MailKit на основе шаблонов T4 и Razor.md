# Создание и отправка писем с помощью MailKit

## Подготовительные действия
1. Установить NuGet-пакет MailKit
2. Дополнительно установить NuGet-пакет System.CodeDom от Microsoft. Неоходимо для текстовых шаблонов T4.
3. Выполнить необходимые регистрации в файле Startup.cs:

```C#

public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddRazorPages();
}

...

public void ConfigureContainer(IUnityContainer container)
{
    ...
    var emailOptions = new MailConfigurations.EmailOptions();
    Configuration.GetSection("Email").Bind(emailOptions);
    container.RegisterInstance(emailOptions);
    
    container.RegisterType<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
    container.RegisterType<IEmailSender, MailKitEmailService>();
}
```
EmailOptions - класс для настройки отправки электронных писем:
- Адрес smtp-сервера.
- Порт smtp-сервера.
- Домен для авторизации на почтовом сервере.
- Логин для авторизации на почтовом сервере.
- Пароль для авторизации на почтовом сервере.
- Вкл./выкл. использования SSL.
- Вкл./выкл. проверки отзыва сертификата.

Настройки отправки электронных писем подгружаются из секции "Email" файла appsettings.json.

RazorViewToStringRenderer - сервис для рендеринга шаблона Razor Pages.

MailKitEmailService - сервис отправки электронных сообщений.



## Использование текстового шаблона Т4
### Настройка шаблона электронного письма

В папке \"\Templates\MailTemplates\T4\" расположены:
- T4MailTemplate.tt - шаблон электронного письма на базе текстового шаблона Т4.
- T4MailTemplateProperties.cs - модель данных используемая в тестовом шаблоне Т4.

Более подробно познакомиться с правилами создания шаблонов T4 можно в этом [справочном материале](https://learn.microsoft.com/ru-ru/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2022).


### Отправка электронного письма
Для отправки письма с использованием текстового шаблона T4 используется метод 
[SendT4Email](https://github.com/Flexberry/Flexberry.AsyncOpenXmlReports.Sample/blob/33398ca91e23c8df6cc757f497689dfeaae376f5/src/AsyncOpenXmlReportsSample/ODataBackend/Services/MailKitEmailService.cs#L37) класса MailKitEmailService, в который необходимо передать:

- Электронный адрес отправителя письма (from).
- Электронный адрес получателя письма (to).
- Электронный адрес получателя копии письма (copyTo).
- Тему письма (subject).
- Текстовое содержимое письма (body).
- Прикрепляемые изображения для отображения в теле письма (bodyAttachments). В случае отсутствия, необходимо передать null.
- Имя прикрепляемого файла (fileName). В случае отсутствия, необходимо передать пустую строку.
- Содержимое прикрепляемого файла (fileBody). В случае отсутствия, необходимо передать null.


## Использование шаблона с синтаксисом Razor
### Настройка шаблона электронного письма
В папке \"\Templates\MailTemplates\RazorPages\" расположены:

- MailTemplateLayout.cshtml - макет шаблона электрнного письма
- RazorPagesMailTemplate.cshtml - представление с содержательной частью электронного письма.
- RazorPagesMailTemplate.cshtml.cs - модель данных, связанная с представлением.

Более подробно познакомиться с синтаксисом разметки Razor можно в этом [справочном материале](https://learn.microsoft.com/ru-ru/aspnet/core/mvc/views/razor?view=aspnetcore-3.1).

### Отправка электронного письма
Для отправки письма с использованием текстового шаблона T4 используется метод 
[SendRazorPagesEmail]() класса MailKitEmailService, в который необходимо передать:

- Электронный адрес отправителя письма (from).
- Электронный адрес получателя письма (to).
- Электронный адрес получателя копии письма (copyTo).
- Тему письма (subject).
- Текстовое содержимое письма (body).
- Прикрепляемые изображения для отображения в теле письма (bodyAttachments). В случае отсутствия, необходимо передать null.
- Имя прикрепляемого файла (fileName). В случае отсутствия, необходимо передать пустую строку.
- Содержимое прикрепляемого файла (fileBody). В случае отсутствия, необходимо передать null.