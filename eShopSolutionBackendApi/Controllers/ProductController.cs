using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolutionBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _pubicProductService;
        public ProductController (IPublicProductService pubicProductService)
        {
            _pubicProductService = pubicProductService;
        }
        [HttpGet]
        public async Task<IActionResult> Get ()
        {
            var product = await _pubicProductService.GetAll();
              return Ok(product);
        }
    }
}