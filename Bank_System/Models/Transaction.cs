using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_System.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }

    public class Transaction
    {
        public int Id { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Сума")]
        public decimal Amount { get; set; }

        [Display(Name = "Тип операції")]
        public TransactionType Type { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Опис")]
        public string Description { get; set; } = string.Empty;
    }
}
