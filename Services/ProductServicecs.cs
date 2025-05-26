using Microsoft.EntityFrameworkCore;
using Product_Details.DTO;
using WebApiTest.Context;
using WebApiTest.Entities;
using WebApiTest.Interface;

namespace WebApiTest.Services
{
    public class ProductServicecs : IProdut
    {    
        private readonly AppDbContext _context;
        public ProductServicecs(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            var CatId = 4;
            var ProductWithCat = await _context.products.FromSqlInterpolated($"EXEC SP_GetProductByCategoryId @ProductCategoryId={CatId}").
                ToListAsync(); /// use store procedure 
            

            return await _context.products.ToListAsync(); 
        }

        public async Task<Product> AddProduct(Product product)//product is instant
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> UpdateProductById(int id, Product product)  //?is nullable ?using null value
        { 
            var existingProduct=await _context.products.FindAsync(id);//findasync work only id
            if (existingProduct==null)
            {
                return null;
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductCategory = product.ProductCategory;
            existingProduct .Price= product.Price;
            existingProduct.isActive = product.isActive;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
        //delete
        public async Task<string> DeleteProduct(int id)
        {
            var Existingproduct = await _context.products.FindAsync(id);
            if (Existingproduct == null)
                return "Unsuccesfull";
            _context.products.Remove(Existingproduct);
            await _context.SaveChangesAsync();
            return "Succesfully";
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.products.FirstOrDefaultAsync(p => p.Id == id);

        }
        public async Task<Product> PartialProductById(int id, Product product)
        {
            return await UpdateProductById(id, product);
        }
        public async Task<Product> AddProductasync(ProductDto PDto)//product is instant
        {
            Product _Product = new Product
            {
                ProductName = PDto.ProductName,
                ProductCategory = PDto.ProductCategory,
                Price = PDto.ProductPrice
            };
             _context.products.Add(_Product);
            await _context.SaveChangesAsync();
            return _Product;

        }
           
    }
}
