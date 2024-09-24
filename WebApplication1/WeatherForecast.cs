using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}




























//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace Vendor_Management_System_Api.Models
{
    public class Vendor
    {
        [Key] // Specifies this property as the primary key
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string VendorCode { get; set; }

        [Required]
        [StringLength(100)]
        public string VendorName { get; set; }

        [Phone]
        public string VendorPhoneNumber { get; set; }

        [EmailAddress]
        public string VendorEmail { get; set; }

        public DateTime VendorCreatedOn { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // Navigation property
    }

    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        [ForeignKey("Currency")]
        public int InvoiceCurrencyId { get; set; }
        public Currency Currency { get; set; } // Navigation property

        [ForeignKey("Vendor")]
        public int InvoiceVendorId { get; set; }
        public Vendor Vendor { get; set; } // Navigation property

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InvoiceAmount { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }

        public bool IsActive { get; set; }
    }

    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrencyName { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // Navigation property
    }
}
