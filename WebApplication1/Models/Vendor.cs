
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApplication1.Models
{
    [Table("Vendors")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int  VendorId { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string VendorPhoneNumber { get; set; }

        public string VendorEmail { get; set; }

        public DateTime VendorCreatedOn { get; set; }

        public bool IsActive { get; set; }

        // Validate method
        public void Validate(List<Vendor> existingVendors)
        {
            if (string.IsNullOrWhiteSpace(VendorCode))
                throw new ArgumentException("Vendor Code cannot be empty.");

            if (string.IsNullOrWhiteSpace(VendorName))
                throw new ArgumentException("Vendor Name cannot be empty.");

            if (VendorName.Any(char.IsDigit))
                throw new ArgumentException("Vendor Name cannot contain numbers.");

            if (VendorPhoneNumber.Length != 10 || !VendorPhoneNumber.All(char.IsDigit))
                throw new ArgumentException("Vendor Phone Number must be 10 digits.");

            if (existingVendors.Any(existingVendor => existingVendor.VendorCode == VendorCode))
                throw new Exception("Vendor Code must be unique.");


        }
    }
}
