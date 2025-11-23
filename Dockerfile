# =============================
# STAGE 1 — Build
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia arquivos de projeto individualmente (melhor cache)
COPY EcoWork.Api/EcoWork.Api.csproj EcoWork.Api/
COPY EcoWork.Tests/EcoWork.Tests.csproj EcoWork.Tests/

# Restaura dependências
RUN dotnet restore EcoWork.Api/EcoWork.Api.csproj

# Copia todo o restante do código
COPY . .

# Publica aplicação
RUN dotnet publish EcoWork.Api/EcoWork.Api.csproj -c Release -o /app/publish

# =============================
# STAGE 2 — Runtime
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia artefatos publicados
COPY --from=build /app/publish .

# Porta obrigatória do Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EcoWork.Api.dll"]