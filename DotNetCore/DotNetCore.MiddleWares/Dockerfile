#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DotNetCore.MiddleWares/DotNetCore.MiddleWares.csproj", "DotNetCore.MiddleWares/"]
RUN dotnet restore "DotNetCore.MiddleWares/DotNetCore.MiddleWares.csproj"
COPY . .
WORKDIR "/src/DotNetCore.MiddleWares"
RUN dotnet build "DotNetCore.MiddleWares.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetCore.MiddleWares.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetCore.MiddleWares.dll"]