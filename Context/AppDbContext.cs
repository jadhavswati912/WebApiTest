using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Product_Details.Entities;
using WebApiTest.Entities;

namespace WebApiTest.Context


{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        
        }
  
          public DbSet<Product>products { get; set; }
          public DbSet<Customer> customers { get; set; }
         
            public DbSet<User> user { get; set; }
            







    }
}
