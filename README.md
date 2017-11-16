# EasyNetQ.Exceptionless
An extension for EasyNetQ which allows you to submit EasyNetQ message consumer errors to [Exceptionless](https://exceptionless.com/).

[![Nuget Downloads](https://img.shields.io/nuget/dt/EasyNetQ.Exceptionless.svg)](https://www.nuget.org/packages/EasyNetQ.Exceptionless/) [![Nuget Version](https://img.shields.io/nuget/v/EasyNetQ.Exceptionless.svg)](https://www.nuget.org/packages/EasyNetQ.Exceptionless/)

### Example

```csharp
using EasyNetQ;
using EasyNetQ.Exceptionless;

var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest", serviceRegister =>
    serviceRegister.EnableExceptionless());
```

### Advanced Use

```csharp
using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Exceptionless;
using Exceptionless;

var exceptionlessClient = ExceptionlessClient.Default; // Or maybe from your DI container

Action<EventBuilder, ConsumerExecutionContext> intercept = (eventBuilder, context) => {
    eventBuilder.AddTags(new [] { "EasyNetQ" });
    eventBuilder.SetProperty("MessageType", context.Properties.Type);

    if (context.Info.Queue == "MyImportantMessages") {
        eventBuilder.MarkAsCritical();
    }
};

var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest", serviceRegister =>
    serviceRegister.EnableExceptionless(exceptionlessClient, intercept));
```