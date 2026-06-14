using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTECommerce.Data;
using JWTECommerce.Models;
using JWTECommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JWTECommerce.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public bool BuyProduct(string name, int quantity)
    {
        if(string.IsNullOrWhiteSpace(name) || quantity <= 0)
            return false;

        var product = _db.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        if(product == null || product.Stock < quantity)
            return false;

        product.Stock -= quantity;
        return Save();
    }

    public bool CreateProduct(Product createProduct)
    {
        if(createProduct == null)
            return false;

        createProduct.CreationDate = DateTime.Now;
        createProduct.UpdateDate = DateTime.Now;

        _db.Products.Add(createProduct);
        return Save();
    }

    public bool DeleteProduct(Product deleteProduct)
    {
        if(deleteProduct == null)
            return false;
        
        _db.Products.Remove(deleteProduct);
        return Save();
    }

    public Product? GetProduct(int id)
    {
        if(id <= 0)
            return null;
            
        return _db.Products
        .Include(c => c.Category)
        .FirstOrDefault(p => p.Id == id);
    }

    public ICollection<Product> GetProductForCategory(int categoryId)
    {
        if(categoryId <= 0)
            return new List<Product>();
        // return _db.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToList();
        return [.. _db.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name)];
    }

    public ICollection<Product> GetProducts()
    {
        // usando Collection Expression y el operador .. (Spread)
        return [.. _db.Products
                .Include(c => c.Category)
                .OrderBy(p => p.Name)];
        
        // Forma tradicional :
        // return _db.Products
        //         .Include(c => c.Category)
        //         .OrderBy(p => p.Name).ToList() ;
        
    }

    public bool ProductExists(int id)
    {
        if(id <= 0)
            return false;
        
        return _db.Products.Any(c => c.Id == id);
    }

    public bool ProductExists(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return false;
        
        return _db.Products.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool Save()
    {
        return _db.SaveChanges() > 0 ? true: false;
    }

    public ICollection<Product> SearchProduct(string name)
    {
        IQueryable<Product> query = _db.Products;
        if(!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        
        return query.OrderBy(p => p.Name).ToList();
    }

    public bool UpdateProduct(Product updateProduct)
    {
        if(updateProduct == null)
            return false;
        
        updateProduct.UpdateDate = DateTime.Now;
        _db.Products.Update(updateProduct);
        return Save();
    }
}