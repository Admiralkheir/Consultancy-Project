# Consultancy Project
Internal management of offices that host psychological counseling centers or expert-client relations
is a web project developed for the execution of the appointment system in digital environment.
The head of the central management unit keeps the records of experts and clients working in the
management on the web side. Consultancy centers with login operations management
facilities are available. The admin panel is available. Entity Framework Core (Code First) is used for data access, and the appointment calendar is built with FullCalendar (create, edit, drag & drop, per-expert filter, conflict check).

C#, ASP.NET Core MVC (.NET 10), MSSQL Server, Entity Framework Core Code First, ASP.NET Core Identity, LINQ,
Bootstrap 5.3 + Bootstrap Icons, FullCalendar 6 (vendored under `wwwroot/lib`), light/dark theme.

## Running

### With Docker (recommended)

```bash
cd OfficeAppointmentSoftware
docker compose up --build
```

Open http://localhost:8080 and sign in with `admin` / `Admin123!` (set `ADMIN_PASSWORD` to change it).
The admin creates offices from **Kullanıcı İşlemleri** (sign-up); each office then signs in with its own user.
SQL Server is exposed on `localhost,14333` (`sa` / `MSSQL_SA_PASSWORD`, default `Local_Dev_Passw0rd`).
Migrations are applied and the roles are seeded on startup.

### Locally

Requires the .NET 10 SDK and a SQL Server instance.

```bash
cd OfficeAppointmentSoftware/OfficeAppointmentSoftware.MVC.WebUI
dotnet user-secrets set "ConnectionStrings:AppointmentDBContext" "Server=localhost,1433;Database=AppointmentDB;User Id=sa;Password=<password>;TrustServerCertificate=True"
dotnet user-secrets set "Seed:AdminPassword" "<admin password>"
dotnet run
```

The contact form sends mail only when the `Smtp` section is configured (keep `Smtp:Password` in user secrets or environment variables).

### Migrations

There are two contexts in the same database: `AppointmentDBContext` (DAL) and `IdentityContext` (WebUI).

```bash
cd OfficeAppointmentSoftware
dotnet tool restore
dotnet ef migrations add <Name> -p OfficeAppointmentSoftware.DAL -s OfficeAppointmentSoftware.MVC.WebUI -c AppointmentDBContext -o Migrations
dotnet ef migrations add <Name> -p OfficeAppointmentSoftware.MVC.WebUI -s OfficeAppointmentSoftware.MVC.WebUI -c IdentityContext -o Migrations
```

![58515935-139fa400-81af-11e9-86d2-d3d33f16fd41](https://user-images.githubusercontent.com/48483419/58797910-dd787f00-8609-11e9-8a9e-40346dcdbeb5.jpg)
![58515934-139fa400-81af-11e9-9b0f-a4d5b3e51dfd](https://user-images.githubusercontent.com/48483419/58797915-dfdad900-8609-11e9-8ff6-4dfab54e8856.jpg)
![58515937-139fa400-81af-11e9-957a-205faef37150](https://user-images.githubusercontent.com/48483419/58797920-e1a49c80-8609-11e9-938d-d9f05ea243ad.jpg)
![58515938-139fa400-81af-11e9-8ba3-214ce3aeaacc](https://user-images.githubusercontent.com/48483419/58797923-e36e6000-8609-11e9-9db7-b0df835d7494.jpg)
![58515939-14383a80-81af-11e9-9fdc-ad8298f4bb60](https://user-images.githubusercontent.com/48483419/58797926-e5382380-8609-11e9-918c-032690f60548.jpg)
![58515940-14383a80-81af-11e9-858e-36944d33b515](https://user-images.githubusercontent.com/48483419/58797932-e8331400-8609-11e9-8dac-8f51b3683027.jpg)
![58515941-14383a80-81af-11e9-850b-a765c3b6f80e](https://user-images.githubusercontent.com/48483419/58797940-ea956e00-8609-11e9-9fdd-e67823cb78af.jpg)
![58515945-15696780-81af-11e9-9c2f-2c14aa14eb5e](https://user-images.githubusercontent.com/48483419/58797942-ed905e80-8609-11e9-9649-137e85fb10ab.jpg)
![58515946-15696780-81af-11e9-982b-8c6a28c17023](https://user-images.githubusercontent.com/48483419/58797943-f08b4f00-8609-11e9-83e9-3c5b50bf8f10.jpg)
![58515947-1601fe00-81af-11e9-80f7-325e1e81298a](https://user-images.githubusercontent.com/48483419/58797953-f8e38a00-8609-11e9-8a89-a403725080b3.jpg)
![58515948-1601fe00-81af-11e9-8fb3-4c90e959a4f6](https://user-images.githubusercontent.com/48483419/58797946-f4b76c80-8609-11e9-8f81-4edf0666f15e.jpg)

