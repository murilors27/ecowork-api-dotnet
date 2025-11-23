# =============================
# STAGE 1 — Build
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo
COPY . .

# Restaura dependências
RUN dotnet restore "EcoWork.Api.csproj"

# Compila
RUN dotnet publish "EcoWork.Api.csproj" -c Release -o /app/publish

# =============================
# STAGE 2 — Runtime
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Porta padrão
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "EcoWork.Api.dll"]