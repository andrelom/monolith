#
# Build

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

COPY src/ ./

RUN dotnet publish Monolith.Web.Identity -c Release -o ./publish --runtime alpine-x64 --self-contained true /p:PublishTrimmed=true

#
# Main

FROM alpine:latest

RUN apk add --no-cache libstdc++ libintl icu

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["./Monolith.Web.Identity", "--urls", "http://0.0.0.0:80"]
