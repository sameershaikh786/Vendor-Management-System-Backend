using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.NewFolder
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        [ForeignKey("CurrencyId")]
        public int CurrencyId { get; set; }

     

        [ForeignKey("VendorId")]
        public int VendorId { get; set; }


        [Required]
        public decimal InvoiceAmount { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public bool IsActive { get; set; }

    }
}
