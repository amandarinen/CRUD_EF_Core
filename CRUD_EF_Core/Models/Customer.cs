using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a customer.
    /// </summary>
    public class Customer
    {
        // PK, the unique identifier for the customer.
        public int CustomerId { get; set; }

        // Customers name.
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        private string? _email;

        // Customers email address. Value is stored in encrypted form in the database and automatically decrypted when read.
        [Required, MaxLength(100)]
        public string? Email 
        { 
            get => _email is null? null : EncryptionHelper.Decrypt(_email); 
            set => _email = string.IsNullOrEmpty(value) ? null : EncryptionHelper.Encrypt(value); 
        } 
        
        // Customers city.
        public string? City { get; set; }

        // A collection of orders associated with this customer.
        public List<Order> Orders { get; set; } = new();
    }
}