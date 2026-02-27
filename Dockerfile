FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
    
WORKDIR /app
EXPOSE 5036

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ProjectTemplate/ProjectTemplate.csproj", "./"]
RUN dotnet restore "ProjectTemplate.csproj"
COPY ProjectTemplate/ .

FROM build AS publish
# To better performance, always will be Release, and in the docker-compose file, we will change the ASPNETCORE_ENVIRONMENT to what we want.
RUN dotnet publish "ProjectTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
