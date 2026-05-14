FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TinhLuongService/TinhLuongService.csproj", "TinhLuongService/"]
RUN dotnet restore "TinhLuongService/TinhLuongService.csproj"
COPY . .
WORKDIR "/src/TinhLuongService"
RUN dotnet build "TinhLuongService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TinhLuongService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8082
ENTRYPOINT ["dotnet", "TinhLuongService.dll"]
