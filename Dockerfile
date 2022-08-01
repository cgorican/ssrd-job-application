FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS build
WORKDIR /src
COPY ["SSRD.csproj", "."]
RUN dotnet restore "./SSRD.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SSRD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSRD.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSRD.dll"]