FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY webapphybridconn/webapphybridconn.csproj webapphybridconn/
RUN dotnet restore webapphybridconn/webapphybridconn.csproj
COPY . .
WORKDIR /src/webapphybridconn
RUN dotnet build webapphybridconn.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish webapphybridconn.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "webapphybridconn.dll"]
