using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos;
using Shared.Enums;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        //Endpoint ==> getallProducts
        [HttpGet ()] //baseurl/api/products
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProductsAsync([FromQuery]ProductSpecificationsParameters parameters)
        {
          var products=  await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);


        }


        //Endpoint ==> getProductById/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResultDto>> getProductAsync(int id)
        {
          var product= await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }



        //Endpoint ==> getProductBrands/Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
        {
            var brands =await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }



        //Endpoint ==> getProductTypes/Types
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync()
        {
            var types= await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
