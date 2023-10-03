using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CreatedAtRoute.Result.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Dictionary<string, string> _products;

        public ProductsController(Dictionary<string, string> products)
        {
            _products = products;
        }

        [HttpGet]
        public IActionResult GetAllProducts() => Ok(_products);

        [HttpGet(template: "{id}", Name = nameof(GetOneProductbyId))]
        public IActionResult GetOneProductbyId([FromRoute] string id)
        {
            var product = _products.SingleOrDefault(p => p.Key.Equals(id));

            if (product.Equals(default(KeyValuePair<string, string>)))
                return NotFound("ürün bulunamadı");

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateOneProduct([FromBody] string product)
        {
            _products.Add(Guid.NewGuid().ToString(), product);
            var createdProduct = _products.Last();

            // routeName parametresine GetOneProductById action metodun route name değeri geçilmelidir.
            // routeName parametresine geçilen değere göre hedef action metot için belirlenen route pattern a erişim sağlanır.
            return CreatedAtRoute(routeName: nameof(GetOneProductbyId), routeValues: new { id = createdProduct.Key }, value: createdProduct.Value);
        }
    }
}