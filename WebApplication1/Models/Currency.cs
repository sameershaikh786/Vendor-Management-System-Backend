using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vendor_Management_System_Api.Models;

namespace WebApplication1.Models
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        public void Validate(List<Currency> existingCurrencies)
        {
            if (string.IsNullOrWhiteSpace(CurrencyName))
            {
                throw new ArgumentException("Currency Name cannot be empty.");
            }

            if (CurrencyName.Any(char.IsDigit))
            {
                throw new ArgumentException("Currency Name cannot contain numbers.");
            }

            if (string.IsNullOrWhiteSpace(CurrencyCode))
            {
                throw new ArgumentException("Currency Code cannot be empty.");
            }

            if (CurrencyCode.Any(char.IsDigit) || CurrencyCode.Length > 3)
            {
                throw new ArgumentException("Currency Code must be 3 characters long and cannot contain numbers.");
            }

            if (existingCurrencies.Any(c => c.CurrencyCode.Equals(CurrencyCode, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Currency Code already exists.");
            }

           
        }

    }
}
