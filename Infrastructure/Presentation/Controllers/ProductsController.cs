using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using Shared.ErrorModels;

namespace Presentation.Controllers
{
  
    public class ProductsController(IServiceManager _serviceManager) : ApiController
    {

        [ProducesResponseType(typeof(ProductResultDto), StatusCodes.Status200OK)]
        //Endpoint ==> getallProducts
        
        [HttpGet ()] //baseurl/api/products
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProductsAsync([FromQuery]ProductSpecificationsParameters parameters)
        {
          var products=  await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);


        }

        [ProducesResponseType(typeof(ProductResultDto),StatusCodes.Status200OK)]
        

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
