FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5001
EXPOSE 5000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NetCoreBackend/NetCoreBackend.csproj", "NetCoreBackend/"]
RUN dotnet restore "NetCoreBackend/NetCoreBackend.csproj"
COPY . .
WORKDIR "/src/NetCoreBackend"
RUN dotnet build "NetCoreBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCoreBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreBackend.dll"]
