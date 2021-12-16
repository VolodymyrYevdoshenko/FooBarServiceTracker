# FooBarServiceTracker
REST API with CRUD operation for monitoring of services. 

Application architecture built in three layers: 

- Data Access
Entity Framework context with Services DB Set. This collection represents appropriate table in DB. After modifying this collection and calling SaveChanges method, SQL queries will be run by EF. 
- Business logic
Service class that contains all business logic regarding service maintenance. It uses DB context for data modification.
- API/Controllers 
Web API controller with endpoints for service data. It uses service via abstraction and using entities (POCO classes). For connection with client, it maps entities into DTOs (data transfer objects) with information only needed for client. Endpoints returns only appropriate status codes like 200, 202, 404.
Validation of input DTOs is done using Action Filter. In case of invalid data, it will return 400 code. 
Also, Swagger is enabled for simple API testing. Swagger.json is manually added to the root folder for viewing.

  As a database for this application should be used any DB using MS SQL Server and with one table called Services.

Possible improvements for future: 

* Divide API project into several projects by layers. Currently it is made through the folders. But in future it will be useful to decrease coupling of components.
* Add general exception handling mechanism. Currently in case of any exception application will return 500 status codes. Ideally need to return more appropriate status codes with messages.
* Think about security. Add some authorization to requests.
* Move connection strings from config file (appsettings.json) to Environment Variables or another appropriate storage. 
* Add cancellation tokens to async methods. It is one of best practices for asynchronous applications and is very helpful. (https://andrewlock.net/using-cancellationtokens-in-asp-net-core-mvc-controllers/)
* Think about handling of data update locking. For example, look at optimistic concurrency strategies (https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency).
* Think about caching of get requests for performance and scalability improvement (https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-6.0).


 Deployment of application: 

* Create a SQL server database or use existing and add table Services. See Services.sql in the root folder for example. 
* Put connection string of DB to the appsettings.json file in “ServiceDbContext” sections.
 
 To run the application: 
- Run command “dotnet run” in command line inside API folder.
- Run docker container using dockerfile from API. 

* Go to https://localhost:7032/swagger/index.html to use API via Swagger.

