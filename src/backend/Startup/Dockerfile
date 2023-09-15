FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Startup/Startup.csproj", "Startup/"]
COPY ["Accounts/Accounts.csproj", "Accounts/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Startup/Startup.csproj"
COPY . .
WORKDIR "/src/Startup"
RUN dotnet build "Startup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Startup.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Startup.dll"]