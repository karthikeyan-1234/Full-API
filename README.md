# Full-API
API with SignalR, Localization, Caching, Logging, JWT, Identity Server, Session Management, Multi Tenancy. [Angular FE Repo https://github.com/karthikeyan-1234/EmployeeAPP_D281221]
The Front end which consumes this API is in repo https://github.com/karthikeyan-1234/EmployeeAPP_D281221.
To Use :

1. Register a Tenant with a unique TenantID. [api/Authenticate/register-tenant]
2. Register an Admin by providing the required details along with a valid TenantID. [api/Authenticate/register-admin]
3. Login by providing just the Username and Password [api/Authenticate/login].
4. Above call with return a JWT token and a refresh token. Use them to call the Application APIs.
5. SessionMiddleWare component of the application API will extract the TenantID and EmailID from the user claims present in the httpContext and store them in separate sessions.
6. In the OnConfiguring method of the DBContext, the TenantID is extracted from Session
7. In the OnModelCreating method of the DBContext, use the TenantID to apply a global filter, using [modelBuilder.Entity<Employee>().HasQueryFilter(e => e.TenantId == TenantID)].
