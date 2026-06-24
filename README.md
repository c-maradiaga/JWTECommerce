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


Analizaremos el proceso dividiéndolo en sus dos grandes fases: **Validación de Credenciales** y **Generación del JWT**.
---

## 🛠️ Fase 1: Validación de Credenciales

En los primeros tres bloques del código, el sistema actúa como un filtro de seguridad para asegurarse de que el usuario es quien dice ser antes de entregarle una "llave" (el token).

* **1. Validación Inicial 🛑:** Comprueba que el `Username` no viaje vacío o con espacios en blanco usando `string.IsNullOrWhiteSpace`. Si falla, corta el flujo inmediatamente devolviendo un DTO con el mensaje de error.
* **2. Búsqueda en Base de Datos 🔍:** Busca al usuario utilizando Entity Framework (`_db.Users.FirstOrDefaultAsync`). Nota cómo usa `.ToLower().Trim()` en ambos lados para evitar problemas con mayúsculas o espacios accidentales. Si no existe, frena el proceso.
* **3. Verificación de la Contraseña 🔐:** Las contraseñas nunca deben guardarse en texto plano. Aquí se usa la librería **BCrypt** (`BCrypt.Verify`) para comparar la contraseña que envía el cliente con el hash seguro que está guardado en la base de datos. Si no coinciden, se rechaza el acceso.

---

## 🎟️ Fase 2: Creación del JWT (JSON Web Token)

Una vez que sabemos que el usuario es válido, el código prepara y firma el token digital.

* **4. Configuración del Token 📝:** Aquí se define la estructura del token usando un `SecurityTokenDescriptor`:
* **Claims (Demandas) 👤:** Son los datos de identidad que viajarán *dentro* del token (ID, nombre de usuario y su rol).
* **Expiración ⏳:** Se define que el token será válido solo durante las próximas 2 horas (`DateTime.UtcNow.AddHours(2)`).
* **Firma Digital 🖋️:** Convierte tu palabra clave (`_secretKey`) en bytes y firma el token usando un algoritmo simétrico (`SecurityAlgorithms.HmacSha256Signature`). Esto garantiza que nadie pueda alterar el token sin conocer la clave secreta del servidor.


* **5. Emisión y Respuesta 🎉:** El `JwtSecurityTokenHandler` crea el objeto del token y luego `handlerToken.WriteToken(token)` lo convierte en la cadena de texto compacta (separada por tres puntos) que el cliente guardará para sus futuras peticiones.

---

## 🗺️ Infografía del Flujo de Login
Para ayudarte a recordar visualmente el orden de las operaciones, puedes seguir este mapa conceptual del método:
```
[Cliente envía LoginDto] 
       │
       ▼
┌────────────────────────────────────────┐
│ 1. ¿El Username tiene texto válido?    │ ❌ No ──► [Retorna Error]
└────────────────────────────────────────┘
       │ Sí
       ▼
┌────────────────────────────────────────┐
│ 2. Buscar usuario en Base de Datos     │ ❌ No existe ──► [Retorna Error]
└────────────────────────────────────────┘
       │ Existe usuario
       ▼
┌────────────────────────────────────────┐
│ 3. Verificar Password con BCrypt       │ ❌ No coincide ──► [Retorna Error]
└────────────────────────────────────────┘
       │ Contraseña Correcta
       ▼
┌────────────────────────────────────────┐
│ 4. Configurar Descriptor de JWT        │ ⚙️ Agrega Claims (ID, Rol) y
└────────────────────────────────────────┘    tiempo de expiración.
       │
       ▼
┌────────────────────────────────────────┐
│ 5. Firmar, Generar y Escribir Token    │ 🔑 Usa la clave secreta
└────────────────────────────────────────┘
       │
       ▼
[Retorna Respuesta Exitosa con JWT y Datos]
```

---
### CORS

Cross-Origin Resurce Sharing para asegurarse de que la API pueda ser consumida desde otras aplicacions o dominios.
* Permite o restring las solicitudes HTTP entre sitios web de diferentes origenes.
* Por defect, los navegadores bloquean las solicitudes cross-origin.
* Se aplica principalmente en llamadas a APIs desde clientes Web

CORS entra en acción en el navegador, no en el servidor ni en el ciente directamente.

¿Cómo habilitar CORS en una API?
* Se configura en el servidor para permitir solicitudes desde orígenes específicos.
* Se puede aplicar globalmente o por controlador.

```
builder.Services.AddCors(options => 
{
       options.AddPolicy("AllowFrontend",
                         policy => policy.WithOrigins("http://otrohost:4000")
                                         .AllowAnyMethod()
                                         .AllowAnyHeader());
});

app.UserCors("AllowFrontEnd");
```


## License

This project is licensed under the MIT License - see the LICENSE file for details.
