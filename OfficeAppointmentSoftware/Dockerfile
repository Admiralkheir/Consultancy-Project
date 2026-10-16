FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so the package layer is cached between code changes.
COPY Directory.Build.props ./
COPY OfficeAppointmentSoftware.Entities/OfficeAppointmentSoftware.Entities.csproj OfficeAppointmentSoftware.Entities/
COPY OfficeAppointmentSoftware.DAL/OfficeAppointmentSoftware.DAL.csproj OfficeAppointmentSoftware.DAL/
COPY OfficeAppointmentSoftware.BLL/OfficeAppointmentSoftware.BLL.csproj OfficeAppointmentSoftware.BLL/
COPY OfficeAppointmentSoftware.MVC.WebUI/OfficeAppointmentSoftware.MVC.WebUI.csproj OfficeAppointmentSoftware.MVC.WebUI/
RUN dotnet restore OfficeAppointmentSoftware.MVC.WebUI/OfficeAppointmentSoftware.MVC.WebUI.csproj

COPY . .
RUN dotnet publish OfficeAppointmentSoftware.MVC.WebUI/OfficeAppointmentSoftware.MVC.WebUI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Uploaded photos are written under wwwroot, so the non-root user needs write access there.
RUN mkdir -p wwwroot/Areas/Admin/Files/Images wwwroot/Areas/Ofiice/image \
    && chown -R app:app wwwroot/Areas/Admin/Files wwwroot/Areas/Ofiice/image
USER app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "OfficeAppointmentSoftware.MVC.WebUI.dll"]
