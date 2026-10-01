using System.ComponentModel.DataAnnotations;

namespace Bank_System.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your last name")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }

        public List<BankAccount> BankAccounts { get; set; } = new();
        public string FullName => $"{FirstName} {LastName}";

    }
}
