using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        private string? _email;

        [Required, MaxLength(100)]
        public string? Email 
        { 
            get => _email is null? null : EncryptionHelper.Decrypt(_email); 
            set => _email = string.IsNullOrEmpty(value) ? null : EncryptionHelper.Encrypt(value); 
        } 
        
        public string? City { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}