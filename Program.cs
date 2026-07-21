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

var builder = WebApplication.CreateBuilder(args);

//? Add services to the container. Agregando los Services:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

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
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "JWTECommerce API",
//         Version = "v1",
//         Description = "API para gestión de productos, categorías y usuarios con autenticación JWT."
//     });

//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
//     });

//     options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecuritySchemeReference("Bearer", document, "Bearer"),
//             new List<string>()
//         }
//     });
// });



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

//! Definiendo que se va a utilizar las politica de CORS:
//app.UseCors("AllowSpecificOrigin");
app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseAuthentication(); // Agregando el middleware de Autenticación, para que se pueda validar el token en cada request. Se debe agregar antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
