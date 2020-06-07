
Docker for Azure App Service - Linux
https://github.com/terraform-providers/terraform-provider-azurerm/tree/master/examples/app-service/docker-basic

Docs on connecting to SQL using Managed Service Identity
https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-connect-msi
https://benfoster.io/blog/serilog-best-practices/

# Items in this template
* Serilog with LoggerFactory
* NimbleConfig
* MassTransit for messaging
* Security headers
* Exception handling
* Health Checks
* Feature folders
* EnvironmentType vs Environment 
* Auto generated Swagger client
* Swagger with NSwag
* Package lock files
* Message contracts class library
* Nugets for MessageContracts and HTTPClient
* Build script (Local uses same script as CI)
* LINQPad script for replacing names to new project
* Test Log enricher from infrastructure folder
* Convention tests
    * Async method naming
    * DateTime usage
    * Only use ILogger from Serilog not MEL
    * MvcController conventions
    * Settings conventions
    * MessageContracts are immutable

# Items to go
* Local dev
    * RabbitMQ (MassTransit)
    * Azure Storage
    * SQL Server
* Test DbUp project & Replacer for DB Name
* Finish wiring in CorrelationId
* Check exception handling with Ajax
* Hook up db work EF/Insight.Database with Managed service identity
* Demo pages built in razor / vue
* Demo / wire in Scrutor https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
* Web API Conventions https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-3.1
* https://fluentvalidation.net/ for controllers
* Compile on linux with docker
* Deploy with Octopus
* https://andrewlock.net/setting-global-authorization-policies-using-the-defaultpolicy-and-the-fallbackpolicy-in-aspnet-core-3/

# Questions
* Do we need auto generated TypeScript client? https://github.com/sanderaernouts/autogenerate-api-client-with-nswag
