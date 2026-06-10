# JWT E-Commerce API

A .NET-based e-commerce API with JWT (JSON Web Token) authentication.

## Overview

This project is a RESTful API built with ASP.NET Core that implements JWT-based authentication for secure e-commerce transactions. It provides endpoints for product management, orders, and user authentication.

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

## License

This project is licensed under the MIT License - see the LICENSE file for details.
