FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["PniaApi/PniaApi.csproj", "PniaApi/"]
ADD ["PniaApi/prefixes.txt", "PniaApi/app/"]
RUN dotnet restore "PniaApi/PniaApi.csproj"
COPY . .
WORKDIR "/src/PniaApi"
RUN dotnet build "PniaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PniaApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY prefixes.txt /app/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PniaApi.dll"]