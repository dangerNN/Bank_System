using Bank_System.Models;

namespace Bank_System.ViewModels
{
    public class AccountDetailsViewModel
    {
        public BankAccount Account { get; set; } = null!;
        public List<Transaction> Transactions { get; set; } = new();
        public DateTime? SearchDate { get; set; }
        public TransactionType? FilterType { get; set; }
        public int TotalTransactionsCount { get; set; }
        public decimal TotalDepositsSum { get; set; }
        public decimal TotalWithdrawalsSum { get; set; }
    }
}
