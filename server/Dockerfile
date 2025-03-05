FROM mcr.microsoft.com/dotnet/sdk:8.0.405-alpine3.21 AS build-env
WORKDIR /app
COPY . .
RUN dotnet publish src \
    -c Release \
    -o bin \
    --sc false \
    -p:AssemblyName=server \
    -p:DebugType=none \
    -p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime:8.0.12-alpine3.21
WORKDIR /app
COPY --from=build-env /app/bin/server .
ENTRYPOINT ["./server"]
EXPOSE 14514
