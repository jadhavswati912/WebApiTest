
using System.ComponentModel.DataAnnotations;

    namespace Product_Details.DTO
{
    public class ProductDto
    {
        [Required(ErrorMessage ="Product Name is required")]
        public  string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public double ProductPrice { get; set; }
    }
}
