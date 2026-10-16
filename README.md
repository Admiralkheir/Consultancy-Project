# Consultancy Project
Internal management of offices that host psychological counseling centers or expert-client relations
is a web project developed for the execution of the appointment system in digital environment.
The head of the central management unit keeps the records of experts and clients working in the
management on the web side. Consultancy centers with login operations management
facilities are available. The admin panel is available. Entity Framework Core (Code First) is used for data access, and the appointment calendar is built with FullCalendar (create, edit, drag & drop, per-expert filter, conflict check).

C#, ASP.NET Core MVC (.NET 10), MSSQL Server, Entity Framework Core Code First, ASP.NET Core Identity, LINQ,
Bootstrap 5.3 + Bootstrap Icons, FullCalendar 6 (vendored under `wwwroot/lib`), light/dark theme.

## Screenshots

Taken from the running app with fictional demo data.

**Landing page**

![Landing page](docs/screenshots/landing.png)

| Services | Plans |
|---|---|
| ![Services section](docs/screenshots/landing-services.png) | ![Plans section](docs/screenshots/landing-pricing.png) |

**Office panel**

| Dashboard | Appointment calendar |
|---|---|
| ![Office dashboard](docs/screenshots/dashboard.png) | ![Weekly appointment calendar](docs/screenshots/calendar.png) |
| **Appointment form** | **Dark theme** |
| ![Appointment form](docs/screenshots/appointment.png) | ![Calendar in dark theme](docs/screenshots/calendar-dark.png) |
| **Clients** | **Client detail** |
| ![Client list](docs/screenshots/clients.png) | ![Client detail](docs/screenshots/client-detail.png) |
| **Experts** | **Sign in** |
| ![Expert list](docs/screenshots/experts.png) | ![Sign-in page](docs/screenshots/login.png) |

**Admin panel**

![Admin office list](docs/screenshots/admin-offices.png)

**Mobile**

<p>
  <img src="docs/screenshots/mobile-dashboard.png" alt="Dashboard on mobile" width="280" />
  <img src="docs/screenshots/mobile-calendar.png" alt="Calendar on mobile" width="280" />
</p>

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
