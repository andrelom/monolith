#
# Main

FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /sdk

COPY src/ ./src
COPY Monolith.sln ./

RUN dotnet restore .
