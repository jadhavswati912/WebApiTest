using Product_Details.DTO;
using WebApiTest.Entities;

namespace WebApiTest.Interface
{
    public  interface IProdut
    {
        Task<IEnumerable<Product>> GetAllProduct();  
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProductById(int id,Product product);
        Task<string> DeleteProduct(int id);
        Task<Product> GetProductById(int id);
        Task<Product> PartialProductById(int id, Product product);
        Task<Product> AddProductasync (ProductDto PDto);





    }
}
