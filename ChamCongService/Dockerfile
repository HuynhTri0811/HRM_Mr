FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ChamCongService/ChamCongService.csproj", "ChamCongService/"]
RUN dotnet restore "ChamCongService/ChamCongService.csproj"
COPY . .
WORKDIR "/src/ChamCongService"
RUN dotnet build "ChamCongService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChamCongService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8083
ENTRYPOINT ["dotnet", "ChamCongService.dll"]
