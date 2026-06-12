# JWT E-Commerce API

A .NET-based e-commerce API with JWT (JSON Web Token) authentication.

## Overview

This project is a RESTful API built with ASP.NET Core that implements JWT-based authentication for secure e-commerce transactions. It provides endpoints for product management, orders, and user authentication.

[URL del repositorio del proyecto](https://github.com/c-maradiaga/JWTECommerce)
[URL del repositorio del Curso](https://github.com/DevTalles-corp/cs-apiEcommerce/tree/fin-seccion-3)

## Requirements

- .NET 10.0 or later
- Visual Studio Code or Visual Studio 2022+
- A modern web browser for testing (optional)

## Getting Started

### Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd JWTECommerce
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

### Configuration

Update the `appsettings.json` file with your configuration:

```json
{
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpirationMinutes": 60
  }
}
```

For development, use `appsettings.Development.json` to override settings.

### Running the Application

Start the development server:

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or the configured port).

## API Testing

Use the included `JWTECommerce.http` file to test endpoints with REST Client extension in VS Code.

## Project Structure

- **Controllers/** - API endpoint handlers
- **Program.cs** - Application startup and configuration
- **appsettings.json** - Configuration file
- **JWTECommerce.http** - REST API test requests

## Authentication

This API uses JWT tokens for authentication. Include the token in the `Authorization` header:

```
Authorization: Bearer <your-jwt-token>
```


## Creando imagen y contenedor para SQL SERVER

Se utilizó docker para crear un contenedor, usando el archivo docker-compose.


## Creación de Categoria

[DataAnnotation documentación](https://learn.microsoft.com/es-mx/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwith-nrt#primary-key)

### Configuración de la conexión con la base de datos SQL Server.

### Instalación de paquetes necesarios para trabajar con EF Core

### Creación del archivo de contexto (DbContext).

### Configuración de la cadena de conexión en el proyecto en Program.cs.

### Ejecución de la primera migración y actualización de la base de datos

dotnet tool install --global dotnet-ef --version 10
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet-ef migrations add Migracion-Inicial
dotnet-ef database update

## API Categoria

DTO:
*  Ocultar determinadas propiedad qeu los clientes no deben ver
*  Omitir propiedades para reducir el tamaño de la carga.
*  Desacoplar la capa de servicio del nivel de base de datos
*  Tener diferetnes DTOs apra cada version de la API.

* AutoMapper:
Convertir de Category a CategoryDto y de Category a CreateCategoryDto.
Se usa la version de automapper 14.0.0. ya que a partir de la v15, se necesita una licencia
y la declaracion en Program.cs cambia

## Configurando Docker-Compose aislado de otros proyectos 
Con esto podríamos tener varios proyectos corriendo simultáneamente, cada uno con su propia
instancia de SQL Server







## License

This project is licensed under the MIT License - see the LICENSE file for details.
