FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

WORKDIR /src

ADD ./server/WebApp ./

RUN dotnet restore \
&& dotnet publish -c Release -o /dist

# ============================================================
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runner

ARG BASE_DIR=/workspace/www/WebApp

WORKDIR ${BASE_DIR}

COPY --from=builder /dist .

VOLUME ${BASE_DIR}"/appsettings.json"
VOLUME ${BASE_DIR}"/secrets.json"

ENTRYPOINT ["dotnet", "WebApp.dll"]