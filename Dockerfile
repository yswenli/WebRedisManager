# 指定基础镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
 
# 设置工作目录
WORKDIR /app 

# 复制所有文件并生成输出
COPY . ./

RUN dotnet nuget add source https://nuget.cdn.azure.cn/v3/index.json -n azure
RUN dotnet restore && dotnet publish -c Release -o out --os linux
 
# 暴露端口
EXPOSE 80
 
# 设置启动命令
ENTRYPOINT ["dotnet", "SAEA.WebRedisManager.dll"]