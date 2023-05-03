#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Bloggy.Web/Bloggy.Web.csproj", "src/Bloggy.Web/"]
COPY ["src/Bloggy.Data/Bloggy.Data.csproj", "src/Bloggy.Data/"]
COPY ["src/Bloggy.Entities/Bloggy.Entities.csproj", "src/Bloggy.Entities/"]
COPY ["src/Bloggy.Shared/Bloggy.Shared.csproj", "src/Bloggy.Shared/"]
COPY ["src/Bloggy.Service/Bloggy.Service.csproj", "src/Bloggy.Service/"]
RUN dotnet restore "src/Bloggy.Web/Bloggy.Web.csproj"
COPY . .
WORKDIR "/src/src/Bloggy.Web"
RUN dotnet build "Bloggy.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bloggy.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bloggy.Web.dll"]