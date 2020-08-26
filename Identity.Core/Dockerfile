#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Identity.Core/Identity.Core.csproj", "Identity.Core/"]
COPY ["ApiDocument/ApiDocument.csproj", "ApiDocument/"]
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["ErrorHandler/ErrorHandler.csproj", "ErrorHandler/"]
COPY ["Logging/Logging.csproj", "Logging/"]
RUN dotnet restore "Identity.Core/Identity.Core.csproj"
COPY . .
WORKDIR "/src/Identity.Core"
RUN dotnet build "Identity.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Core.dll"]