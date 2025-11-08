# Stage 1: Only for build
ARG TARGETPLATFORM
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY UserRequestsKafkaGenerator.csproj ./
RUN dotnet restore UserRequestsKafkaGenerator.csproj

COPY . .
ARG TARGETARCH
RUN dotnet publish -a $TARGETARCH -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserRequestsKafkaGenerator.dll"]
