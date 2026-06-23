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

## Configurando Docker-Compose aislado de otros proyectos dir
Con esto podríamos tener varios proyectos corriendo simultáneamente, cada uno con su propia
instancia de SQL Server

## Para mostrar la ventana de Scalar (en lugar de swagger)

http://localhost:5062/scalar/v1#tag/categories

## Para usar Swagger 

https://localhost:7185/swagger/index.html ó
http://localhost:5062/swagger/index.html  (el que funcione.)


[Documentacion sobre relaciones](https://learn.microsoft.com/es-mx/ef/core/modeling/relationships)

## Usando Collection Expression y el operador de Propagacion (Spread):
```
public ICollection<Product> GetProducts()
    {
        //return _db.Products.OrderBy(p => p.Name).ToList() ;
        return [.. _db.Products.OrderBy(p => p.Name)];
    }
```
Por qué funciona sin el .ToList():

[]: Los corchetes indican al compilador que estás creando una nueva colección vacía.

..: Este operador toma el resultado de _db.Products.OrderBy(...) (que en este punto es un IEnumerable evaluado de Entity Framework) y "desempaqueta" o esparce sus elementos individualmente dentro de la nueva colección.

Inferencia de tipos (Target typing) 🧠: Esta es la clave de tu pregunta. Como la firma de tu método declara explícitamente que devuelve un ICollection<Product>, el compilador de C# sabe exactamente qué necesita entregar. Al ver los corchetes [], genera automáticamente en segundo plano la estructura de datos más óptima (generalmente un List<Product>) que cumpla con la interfaz ICollection, evitándote escribir el .ToList() de forma explícita.


[Documentación sobre Inyeccion de Dependencias](https://learn.microsoft.com/es-mx/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-9.0&preserve-view=true)

[Documentacion sobre Carga Anticipada](https://learn.microsoft.com/es-mx/ef/core/querying/related-data/eager)

## Realizando el Backup de la Base de Datos:
Estando en una ventana de Powershell
```
docker exec -it sqlserver2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pass123**" -C -Q "BACKUP DATABASE [JWTECommerce] TO DISK = '/var/opt/mssql/data/JWTECommerce.bak'"
```

Con el comando docker cp se pueden transferir archivos entre el contenedor y la máquina física directamente desde la terminal
```
docker cp <nombre_del_contenedor>:<ruta_del_archivo_en_contenedor> <ruta_destino_en_tu_pc>

docker cp sqlserver2022:/var/opt/mssql/data/JWTECommerce.bak c:\Respaldos
```
La ubicación del respaldo dentro del contenedor es /var/opt/mssql/data/JWTECommerce.bak
(El directorio de destino debe existir previamente en la máquina física)

```
En Visual Studio Code, esta característica se conoce como Code Folding (Plegado de código) 📂. Las flechas en el margen izquierdo aparecen al pasar el cursor por encima, pero puedes controlar el estado de todos los métodos usando combinaciones de teclas ⌨️:

Colapsar todos los métodos (Fold All): Presiona Ctrl + K y luego Ctrl + 0 (el número cero) en Windows/Linux, o Cmd + K y luego Cmd + 0 en Mac. Esto dejará todo tu archivo estructurado como en la captura.

Expandir todos los métodos (Unfold All): Presiona Ctrl + K y luego Ctrl + J en Windows/Linux, o Cmd + K y luego Cmd + J en Mac.

Colapsar o expandir solo el bloque actual: Presiona Ctrl + Shift + [ para cerrar el bloque donde está tu cursor, o Ctrl + Shift + ] para volverlo a abrir.
```

### Instalando BCrypt para encriptar las claves de los usuarios:
Package: https://www.nuget.org/packages/BCrypt.Net-Next/ 
```
namespace MyDotNetProject;

using BCryptNet;

// Hash a password
string passwordHash =  BCrypt.HashPassword("my password");

// Verify a password
if(BCrypt.Verify("my password", passwordHash))
{
    // Password is correct
}
```

### Introduccion a JWT
JWT Debugger: https://www.jwt.io/
JWT es un standar abierto para transmitir informacion.
*  Maneja autenticación y Autorización
*  Es autocontenible (El token tiene todo lo necesario para la identificacion de su contenido)
*  Se puede firmar y cifrar

Estructura de un JWT:
   *  El Header (Tipo de token y algoritmo de firma como SHA256, HMAC, etc.)
   *  Payload (Contiene los claims o datos, como el Id de usuario, nombre, roles, etc.)
   *  Signature (Firma digital a partir del header, payload y una clave secreta)









## License

This project is licensed under the MIT License - see the LICENSE file for details.
