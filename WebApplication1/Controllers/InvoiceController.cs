using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly SampleContext _dbContext;

        public InvoiceController(SampleContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("List")]
        public async Task<IActionResult> ListInvoice()
        {
            try
            {
                var invoices = await _dbContext.Invoices.ToListAsync();
                return Ok(invoices);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving invoices.");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddInvoice([FromBody] InvoiceDto newInvoice)
        {
            try
            {
                var invoice = new Invoice()
                {
                    InvoiceNumber = newInvoice.InvoiceNumber,
                    CurrencyId = newInvoice.CurrencyId,
                    VendorId = newInvoice.VendorId,
                    InvoiceAmount = newInvoice.InvoiceAmount,
                    ReceivedDate = DateTime.Now,
                    InvoiceDueDate = newInvoice.InvoiceDueDate,
                    IsActive = true,
                    //vendor = vendor,
                    //currency = currency
                };


                var existingVendors = await _dbContext.Vendors.ToListAsync();
                var existingCurrencies = await _dbContext.Currencies.ToListAsync();

                invoice.Validate(existingVendors, existingCurrencies);
              
                await _dbContext.Invoices.AddAsync(invoice);
                await _dbContext.SaveChangesAsync();

                return Created("", "Invoice added successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while adding the invoice.");
            }
        }

        [HttpDelete("{invoiceId}")]
        public async Task<IActionResult> DeleteInvoices(int invoiceId)
        {
            try
            {
                var invoiceToDelete = await _dbContext.Invoices.FindAsync(invoiceId);

                if (invoiceToDelete == null)
                {
                    return NotFound("Invoice not found!");
                }

                _dbContext.Invoices.Remove(invoiceToDelete);
                await _dbContext.SaveChangesAsync();

                return NoContent(); // Return 204 No Content for successful deletion
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the invoice.");
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditInvoice([FromBody] Invoice obj)
        {
            try
            {
                var invoiceToEdit = await _dbContext.Invoices.FindAsync(obj.InvoiceId);

                if (invoiceToEdit == null)
                {
                    return NotFound("Invoice not found!");
                }

               
                var existingVendors = await _dbContext.Vendors.ToListAsync();
                var existingCurrencies = await _dbContext.Currencies.ToListAsync();

                obj.Validate(existingVendors, existingCurrencies); 

                invoiceToEdit.InvoiceNumber = obj.InvoiceNumber;
                invoiceToEdit.VendorId = obj.VendorId;
                invoiceToEdit.CurrencyId = obj.CurrencyId;
                invoiceToEdit.InvoiceAmount = obj.InvoiceAmount;
                invoiceToEdit.ReceivedDate = obj.ReceivedDate;
                invoiceToEdit.InvoiceDueDate = obj.InvoiceDueDate;
                invoiceToEdit.IsActive = obj.IsActive;

                await _dbContext.SaveChangesAsync();

                return Ok("Invoice updated successfully!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the invoice.");
            }
        }
    }
}
