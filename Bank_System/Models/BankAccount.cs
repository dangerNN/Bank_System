using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace Bank_System.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [Required]
        public string? Currency { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}
