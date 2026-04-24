# 使用官方 .NET 10 SDK 镜像作为构建环境
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SAEA.WebRedisManager/SAEA.WebRedisManager.csproj", "SAEA.WebRedisManager/"]
RUN dotnet restore "SAEA.WebRedisManager/SAEA.WebRedisManager.csproj"
COPY . .
WORKDIR "/src/SAEA.WebRedisManager"
RUN dotnet publish "SAEA.WebRedisManager.csproj" -c Release -o /app/publish --no-restore

# 使用更小的运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY SAEA.WebRedisManager/wwwroot ./wwwroot
EXPOSE 16379
ENTRYPOINT ["dotnet", "SAEA.WebRedisManager.dll"]