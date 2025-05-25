using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Details.DTO;
using Product_Details.Helpers;
using WebApiTest.Context;
using WebApiTest.Entities;
using WebApiTest.Interface;
using WebApiTest.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class ProductController : ControllerBase
    {
        private readonly IProdut _ProductService;

        public ProductController(IProdut ProductService)
        {
            _ProductService = ProductService;
        }
        //get all products
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var ProductList = await _ProductService.GetAllProduct();

            return Ok(ProductList);
        }
        // POST api/<ProductController>
        /// <summary>
        /// Post API test 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// httppost
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var createdProduct = await _ProductService.AddProduct(product);
            return Ok(createdProduct);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductById(int id, Product product)
        {
            var updateProduct = await _ProductService.UpdateProductById(id, product);

            if (updateProduct == null)
                return NotFound();
            return Ok(updateProduct);

        }

        // DELETE api/<ProductController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _ProductService.DeleteProduct(id);
            if (isDeleted == "unsuccesfull")
            {
                return NotFound();
            }
            return Ok("succesfully");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialProductById(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest();
                }

                var ProductData = await _ProductService.GetProductById(id);
                if (ProductData == null)
                {
                    return NotFound();

                }
                patchDoc.ApplyTo(ProductData, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ProductService.PartialProductById(id, ProductData);

                return Ok(ProductData);


            }
            catch (ArgumentException Ex)
            {
                return BadRequest(new { Message = "Invalid parameter", Detail = Ex.Message });
            }
        }
        [HttpPost ("add-new-product")]//custome routing
        public async Task <IActionResult> CreateProducr([FromBody] ProductDto PDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var createdProduct = await _ProductService.AddProductasync(PDto);
            return Ok(createdProduct);
         
        }
    }     
}   

    





