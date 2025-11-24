# Stage 1: Only for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY RateLimiter.Reader.csproj ./
RUN dotnet restore RateLimiter.Reader.csproj

COPY . .
ARG TARGETARCH
RUN dotnet publish -a $TARGETARCH -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RateLimiter.Reader.dll"]
