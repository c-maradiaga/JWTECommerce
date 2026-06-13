using AutoMapper;
using JWTECommerce.Data;
using JWTECommerce.Repository;
using JWTECommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//? Add services to the container. Agregando los Services:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

//? Registrando la D.I. (Interfaz y su respectivo Repositorio)
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddAutoMapper(typeof(Program).Assembly); //! asi se define la v14, porque de la v15 en adelante se necesita un Licence key.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//? la opción para usar la v15 es: builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//? Se quito para usar Swagger: builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//? Se quitaron para usar Swagger
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.MapScalarApiReference();
// }
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Solo redirige a HTTPS si NO estamos en desarrollo
// if (!app.Environment.IsDevelopment())
// {
//     app.UseHttpsRedirection();
// }
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
