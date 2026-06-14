using JWTECommerce.Models;

namespace JWTECommerce.Repository.IRepository;

public interface IProductRepository
{
    
    Product? GetProduct(int id);
    bool ProductExists(int id);
    bool ProductExists(string name);
    ICollection<Product> GetProducts();
    ICollection<Product> GetProductForCategory(int categoryId);
    ICollection<Product> SearchProduct(string name);
    bool CreateProduct(Product createProduct);
    bool UpdateProduct(Product updateProduct);
    bool DeleteProduct(Product deleteProduct);
    bool BuyProduct(string name, int quantity);

    bool Save();
}