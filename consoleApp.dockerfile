FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

WORKDIR /src

# Copy and restore as distinct layers
ADD ./server/ConsoleApp ./

RUN dotnet restore \
&& dotnet publish -c Release -o /dist

# ============================================================
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runner

ARG BASE_DIR=/workspace/www/ConsoleApp

WORKDIR ${BASE_DIR}
# Build runtime image
COPY --from=builder /dist .

ENTRYPOINT ["dotnet", "ConsoleApp.dll"]