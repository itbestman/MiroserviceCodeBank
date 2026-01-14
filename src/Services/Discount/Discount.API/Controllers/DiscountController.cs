using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var discount = await _discountRepository.GetDiscount(productName);
            return Ok(discount);
        }

        [HttpPost(Name ="CreateDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CreateDiscount([FromBody] Coupon coupon)
        {
            var created = await _discountRepository.CreateDiscount(coupon);
            return Ok(created);
        }

        [HttpPut(Name = "UpdateDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
        {
            var updated = await _discountRepository.UpdateDiscount(coupon);
            return Ok(updated);
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            var deleted = await _discountRepository.DeleteDiscount(productName);
            return Ok(deleted);
        }

    }
}
