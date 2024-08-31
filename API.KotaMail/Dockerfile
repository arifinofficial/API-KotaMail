#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API.KotaMail/API.KotaMail.csproj", "API.KotaMail/"]
COPY ["Api.DataAccess/API.DataAccess.csproj", "Api.DataAccess/"]
COPY ["API.Dto/API.Dto.csproj", "API.Dto/"]
COPY ["Framework.Dto/Framework.Dto.csproj", "Framework.Dto/"]
COPY ["API.RepositoryContract/API.RepositoryContract.csproj", "API.RepositoryContract/"]
COPY ["Framework.RepositoryContract/Framework.RepositoryContract.csproj", "Framework.RepositoryContract/"]
COPY ["API.Repository/API.Repository.csproj", "API.Repository/"]
COPY ["Framework.Repository/Framework.Repository.csproj", "Framework.Repository/"]
COPY ["Framework.Core/Framework.Core.csproj", "Framework.Core/"]
COPY ["API.ServiceContract/API.ServiceContract.csproj", "API.ServiceContract/"]
COPY ["Framework.ServiceContract/Framework.ServiceContract.csproj", "Framework.ServiceContract/"]
COPY ["API.Service/API.Service.csproj", "API.Service/"]
COPY ["Framework.Service/Framework.Service.csproj", "Framework.Service/"]
COPY ["Framework.Application/Framework.Application.csproj", "Framework.Application/"]
RUN dotnet restore "./API.KotaMail/./API.KotaMail.csproj"
COPY . .
WORKDIR "/src/API.KotaMail"
RUN dotnet build "./API.KotaMail.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.KotaMail.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.KotaMail.dll"]