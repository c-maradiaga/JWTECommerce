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


}