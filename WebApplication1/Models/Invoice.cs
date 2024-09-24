using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        [ForeignKey("CurrencyId")]
        public int CurrencyId { get; set; }

        //[JsonIgnore]
        public virtual Currency Currency { get; set; }

        [ForeignKey("VendorId")]
        public int VendorId { get; set; }

        //[JsonIgnore]
        public virtual Vendor Vendor { get; set; }

        [Required]
        public decimal InvoiceAmount { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public bool IsActive { get; set; }




        public void Validate(List<Vendor> existingVendors, List<Currency> existingCurrencies)
        {
            if (InvoiceNumber <= 0)
            {
                throw new ArgumentException("Invoice Number must be a positive non-zero number.");
            }

            // Check for CurrencyId only if it is provided
            if (CurrencyId != 0 && !existingCurrencies.Any(c => c.CurrencyId == CurrencyId))
            {
                throw new ArgumentException("Currency ID does not exist. Please add the currency first.");
            }

            // Check for VendorId only if it is provided
            if (VendorId != 0 && !existingVendors.Any(v => v.VendorId == VendorId))
            {
                throw new ArgumentException("Vendor ID does not exist. Please add the vendor first.");
            }

            if (InvoiceAmount <= 0)
            {
                throw new ArgumentException("Invoice Amount must be a positive number.");
            }

            if (InvoiceDueDate < ReceivedDate)
            {
                throw new ArgumentException("Due date cannot be earlier than the received date. Please enter a valid due date.");
            }

            IsActive = true;
        }

    }
}

