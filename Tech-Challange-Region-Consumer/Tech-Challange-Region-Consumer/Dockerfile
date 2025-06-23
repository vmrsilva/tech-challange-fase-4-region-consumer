#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tech-Challange-Region-Consumer/Tech-Challange-Region-Consumer.csproj", "Tech-Challange-Region-Consumer/"]
RUN dotnet restore "./Tech-Challange-Region-Consumer/Tech-Challange-Region-Consumer.csproj"
COPY . .
WORKDIR "/src/Tech-Challange-Region-Consumer"
RUN dotnet build "./Tech-Challange-Region-Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tech-Challange-Region-Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tech-Challange-Region-Consumer.dll"]