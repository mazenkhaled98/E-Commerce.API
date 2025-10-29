using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController(IServiceManager _serviceManager) : ApiController
    {
        //get        //BaseUrl/api/basket
        [HttpGet]
        public async Task<ActionResult> GetBasketByIdAsync(string id)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basket);
        }

        //post
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(basket);
        }

        //delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasketAsync(string id)
        {
            var result = await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent(); //204
        }
    }
}
