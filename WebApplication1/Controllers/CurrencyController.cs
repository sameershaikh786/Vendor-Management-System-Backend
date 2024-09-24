using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1;

namespace Vendor_Management_System_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly SampleContext _dbContext;

        public CurrencyController(SampleContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            try
            {
                var currencies = await _dbContext.Currencies.ToListAsync();
                return Ok(currencies);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving currencies.");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCurrency([FromBody] Currency newCurrency)
        {
            try
            {
                var existingCurrencies = await _dbContext.Currencies.ToListAsync();
                newCurrency.Validate(existingCurrencies);
                await _dbContext.Currencies.AddAsync(newCurrency);
                await _dbContext.SaveChangesAsync();

                return Created("", "Currency added successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while adding the currency.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveCurrencyById(int id)
        {
            try
            {
                var currencyToRemove = await _dbContext.Currencies.FindAsync(id);
                if (currencyToRemove == null)
                {
                    return NotFound("Currency not found!");
                }

                _dbContext.Currencies.Remove(currencyToRemove);
                await _dbContext.SaveChangesAsync();

                return NoContent(); // Return 204 No Content for successful deletion
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the currency.");
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditCurrency([FromBody] Currency updatedCurrency)
        {
            try
            {
                var currencyToEdit = await _dbContext.Currencies.FindAsync(updatedCurrency.CurrencyId);
                if (currencyToEdit == null)
                {
                    return NotFound("Currency not found!");
                }

                currencyToEdit.CurrencyName = updatedCurrency.CurrencyName;
                currencyToEdit.CurrencyCode = updatedCurrency.CurrencyCode;

                await _dbContext.SaveChangesAsync();

                return Ok("Currency updated successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the currency.");
            }
        }
    }
}
