# ETAPA 1: Compilación (aquí se usa el código fuente de forma temporal)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar la app
COPY . ./
RUN dotnet publish -c Release -o out

# ETAPA 2: Imagen Final de Ejecución (¡NO contiene código fuente, solo DLLs!)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Copiamos únicamente los binarios compilados desde la etapa anterior
COPY --from=build-env /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "JWTECommerce.API.dll"]