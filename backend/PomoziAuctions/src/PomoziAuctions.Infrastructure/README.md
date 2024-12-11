## Infrastructure Project

In Clean Architecture, Infrastructure concerns are kept separate from the core business rules (or domain model in DDD).

The only project that should have code concerned with EF, Files, Email, Web Services, Azure/AWS/GCP, etc is Infrastructure.

Infrastructure should depend on Core (and, optionally, Use Cases) where abstractions (interfaces) exist.

Infrastructure classes implement interfaces found in the Core (Use Cases) project(s).

These implementations are wired up at startup using DI. In this case using `Microsoft.Extensions.DependencyInjection` and extension methods defined in each project.

Need help? Check out the sample here:
https://github.com/ardalis/CleanArchitecture/tree/main/sample


## Setting it up on Mac:  
1. Install Docker Desktop  
2. Install Azure Data Studio  
3. Download the latest sql server 2019 image:  
```sudo docker pull mcr.microsoft.com/mssql/server:2019-latest```
4. Run the container from the image with command:  
```sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=some2ComplexPassword" -p 1433:1433 --name sql1 --hostname sql.server  -d  mcr.microsoft.com/mssql/server:2019-latest```
5. In Azure Data Studio, create a new connection:
- Connection type: Select “Microsoft SQL Server” from the dropdown.
- Server: enter ```localhost,1433``` in the field. Make sure to enter the port which you have in the docker configuration.
- Authentication type: Select “SQL login” from the dropdown.
- User name: enter ```sa``` in the field as the user name.
- Password: enter the password that you set in the docker config file. In my case, it is ```some2ComplexPassword```.
- Remember Password: check this checkbox.
6. Change the connection string to:  
```"Server=localhost,1433; Database=PomoziAuctions; Trusted_Connection=false; User=sa;Password=some2ComplexPassword;"```
7. To run migrations and update the database, run the following command from PomoziAuctions folder:  
```dotnet ef database update --context AppDbContext --project src/PomoziAuctions.Infrastructure --startup-project src/PomoziAuctions.Web  
   dotnet ef database update --context IdentityDbContext --project src/PomoziAuctions.Infrastructure --startup-project src/PomoziAuctions.Web 
```

## Useful commands

- dotnet
```
dotnet ef database update 0 --context AppDbContext --project src/PomoziAuctions.Infrastructure --startup-project src/PomoziAuctions.Web  
dotnet ef migrations remove --context AppDbContext --project src/PomoziAuctions.Infrastructure --startup-project src/PomoziAuctions.Web  
dotnet ef migrations add InitialCreate --output-dir Data/Migrations --context AppDbContext --project src/PomoziAuctions.Infrastructure --startup-project src/PomoziAuctions.Web
```

- Visual Studio
```Add-Migration Initial -Context AppDbContext -Project src\PomoziAuctions.Infrastructure -StartupProject src\PomoziAuctions.Web -OutputDir "Data/Migrations"```
```Add-Migration AddSupplierManagerRole -Context IdentityDbContext -Project src\PomoziAuctions.Infrastructure -StartupProject src\PomoziAuctions.Web -OutputDir "Identity/Migrations"```
```Update-Database -Context AppDbContext```
```Update-Database Initial -Context AppDbContext -Project src\PomoziAuctions.Infrastructure -StartupProject src\PomoziAuctions.Web```
```Remove-Migration -Context AppDbContext -Project src\PomoziAuctions.Infrastructure -StartupProject src\PomoziAuctions.Web```
```Update-Database -Context IdentityDbContext```

**Drop DB for testing purposes:**  
```
USE master;
GO
ALTER DATABASE PomoziAuctions 
SET SINGLE_USER 
WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE PomoziAuctions;
```

### Useful links
- https://bigboxcode.com/configure-microsoft-sql-server-using-docker-and-azure-data-studio  
- https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash