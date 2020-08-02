using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VidyaSadhan_API.Services;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly AddressService _addressService;

        public AddressController(ILogger<AddressController> logger, AddressService addressService)
        {
            _logger = logger;
            _addressService = addressService;
        }

        [HttpGet("userId")]
        [ProducesResponseType(typeof(IEnumerable<AddressViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                return Ok(await _addressService.GetAddressByUserId(userId).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured in users", null);
                throw;
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Post(AddressViewModel address)
        {
            try
            {
                return Ok(await _addressService.SaveAddress(address).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured in users", null);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Put(AddressViewModel address)
        {
            try
            {
                return Ok(await _addressService.UpdateAddress(address).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured in users", null);
                throw;
            }
        }

    }
}
