FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY requirements.txt ./
COPY ["MiniTwit.Server/MiniTwit.Server.csproj", "MiniTwit.Server/"]
RUN dotnet restore "MiniTwit.Server/MiniTwit.Server.csproj"
COPY . .
WORKDIR "/src/MiniTwit.Server"
RUN dotnet build "MiniTwit.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniTwit.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniTwit.Server.dll"]
