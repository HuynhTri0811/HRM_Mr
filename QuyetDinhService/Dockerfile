FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuyetDinhService/QuyetDinhService.csproj", "QuyetDinhService/"]
RUN dotnet restore "QuyetDinhService/QuyetDinhService.csproj"
COPY . .
WORKDIR "/src/QuyetDinhService"
RUN dotnet build "QuyetDinhService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuyetDinhService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8084
ENTRYPOINT ["dotnet", "QuyetDinhService.dll"]
