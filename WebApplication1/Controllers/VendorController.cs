using System.Reactive.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly SampleContext _dbContext;

        public VendorController(SampleContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("All-Vendors")]
        public async Task<IActionResult> ListVendor()
        {
            try
            {
                var vendors = await _dbContext.Vendors.ToListAsync();
                return Ok(vendors);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving vendors.");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVendor([FromBody] Vendor newVendor)
        {
            try
            {
                var existingVendors = await _dbContext.Vendors.ToListAsync();
                newVendor.Validate(existingVendors);
                newVendor.IsActive = true;
                newVendor.VendorCreatedOn = DateTime.Now;

                await _dbContext.Vendors.AddAsync(newVendor);
                await _dbContext.SaveChangesAsync();

                return Created("", "Vendor added successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); //400
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while adding the vendor.");
            }
        }

        [HttpDelete("{vendorId}")]
        public async Task<IActionResult> DeleteVendor(int vendorId)
        {
            var vendorToDelete = await _dbContext.Vendors.FindAsync(vendorId);

            if (vendorToDelete == null)
            {
                return NotFound("Vendor not found!");
            }

            var isVendorLinkedToInvoice = await _dbContext.Invoices.AnyAsync(i => i.VendorId == vendorId);
            if (isVendorLinkedToInvoice)
            {
                return Conflict("Cannot delete vendor. There are associated invoices.");
            }

            _dbContext.Vendors.Remove(vendorToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent(); //204
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditVendor([FromBody] Vendor obj)
        {
            var vendorToEdit = await _dbContext.Vendors.FindAsync(obj.VendorId);

            if (vendorToEdit == null)
            {
                return NotFound("Vendor not found!");
            }

            vendorToEdit.VendorCode = obj.VendorCode;
            vendorToEdit.VendorName = obj.VendorName;
            vendorToEdit.VendorPhoneNumber = obj.VendorPhoneNumber;
            vendorToEdit.VendorEmail = obj.VendorEmail;
            vendorToEdit.IsActive = obj.IsActive;

            await _dbContext.SaveChangesAsync();

            return Ok("Vendor updated successfully!"); //200
        }
    }
}
