using System.Text;
using JWTECommerce.Constants;
using JWTECommerce.Data;
using JWTECommerce.Repository;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//? Add services to the container. Agregando los Services:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//? Registrando la D.I. (Interfaz y su respectivo Repositorio)
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddAutoMapper(typeof(Program).Assembly); //! asi se define la v14, porque de la v15 en adelante se necesita un Licence key.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//? la opción para usar la v15 es: builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//* Agregando el servicio de Autenticación JWT:
// trayendo la secret key:
var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrWhiteSpace(secretKey))
    throw new InvalidConfigurationException("El Secret Key No está configurado aún.");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;   // el JwtBearerDefautl se debe instalar el paquete JwtBear
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = true; // --> esto en Preproducción puede ir en false
    options.SaveToken = true; // --> guarda el token en el contexto de ejecución
    options.Authority = "https://localhost:44395/"; //? URL del IdentityServer
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false, // --> En pre-producción puede ir en true, pero se debe configurar el Issuer en el IdentityServer
        ValidateAudience = false
    };
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//? Se quito para usar Swagger: builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//* Agregando autenticación en Swagger:
// Configuración avanzada de Swagger para soportar JWT Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tu API de Fintech/Préstamos",
        Version = "v1",
        Description = "Web API con autenticación JWT Bearer"
    });

    // Definir el esquema de seguridad JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en este formato: Bearer {tu_token_aquí}"
    });
});



//? Agregando CORS:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
     builder =>
     {
         builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
     });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
//? Se quitaron para usar Swagger
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.MapScalarApiReference();
// }

// --- INICIO DE BLOQUE SEGURO DE MIGRACIONES ---
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var logger = services.GetRequiredService<ILogger<Program>>();

//     int maxRetries = 5;
//     int delayInSeconds = 5;

//     for (int retry = 1; retry <= maxRetries; retry++)
//     {
//         try
//         {
//             logger.LogInformation($"Intentando aplicar migraciones (Intento {retry} de {maxRetries})...");
//             var dbContext = services.GetRequiredService<ApplicationDbContext>();
//             dbContext.Database.Migrate();
//             logger.LogInformation("¡Migraciones aplicadas con éxito!");
//             break; // Si se aplicó correctamente, sale del bucle
//         }
//         catch (SqlException ex)
//         {
//             logger.LogWarning($"No se pudo conectar a SQL Server en el intento {retry}: {ex.Message}");
//             if (retry == maxRetries)
//             {
//                 logger.LogError(ex, "Ocurrió un error crítico al aplicar las migraciones tras varios intentos.");
//             }
//             else
//             {
//                 Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
//             }
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Ocurrió un error inesperado al aplicar las migraciones.");
//             break;
//         }
//     }
// }
// --- FIN DE BLOQUE SEGURO DE MIGRACIONES ---



app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTECommerce API v1");
});

// Solo redirige a HTTPS si NO estamos en desarrollo
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


//! Bloque de código para la migración e inicialización de datos al arrancar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Aplica migraciones pendientes y crea la base de datos si no existe
        context.Database.Migrate();

        // Opcional: Aquí puedes llamar a tu método de inicialización de datos (Seeding)
        // DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones a la base de datos.");
    }
}


//! Definiendo que se va a utilizar las politica de CORS:
//app.UseCors("AllowSpecificOrigin");
app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseAuthentication(); // Agregando el middleware de Autenticación, para que se pueda validar el token en cada request. Se debe agregar antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
