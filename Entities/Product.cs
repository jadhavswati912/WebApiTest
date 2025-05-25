namespace WebApiTest.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string  ProductName { get; set; }
        public int ProductCategory { get; set; }
        public double Price { get; set; }
        public bool isActive { get; set; }
    }
}
