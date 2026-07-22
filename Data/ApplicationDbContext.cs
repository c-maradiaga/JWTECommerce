using JWTECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTECommerce.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // no lleva nada aquí.    
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- CONFIGURACIÓN DE DATA SEEDING (DATOS SEMILLA) ---
        // Ejemplo de inserción de datos iniciales en una entidad/tabla
        // NOTA: Es muy importante asignar explícitamente los IDs (Claves primarias) al usar HasData.
        // Seed initial category
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Categoria Inicial",
            },

            new Category
            {
                Id = 2,
                Name = "Categoría 2",
            }
        );

        // Seed initial products (note: include CategoryId and also set the required navigation property)
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Producto Inicial 1",
                Descripcion = "Descripción del producto 1",
                Price = 99.99m,
                ImgURL = "https://via.placeholder.com/150",
                SKU = "PROD-001-BLK-M",
                Stock = 10,
                CreationDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CategoryId = 1, // <--- Únicamente indicamos el ID de la categoría asociada
                Category = null! //  Ya que la propiedad de navegación en la entidad Product esta definida como required
            },
            new Product
            {
                Id = 2,
                Name = "Producto Inicial 1",
                Descripcion = "Descripción del producto 1",
                Price = 99.99m,
                ImgURL = "https://via.placeholder.com/150",
                SKU = "PROD-001-BLK-M",
                Stock = 10,
                CreationDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CategoryId = 1, // <--- Únicamente indicamos el ID de la categoría asociada
                Category = null! // Ya que la propiedad de navegación en la entidad Product esta definida como required 
            }
        );


    }



}