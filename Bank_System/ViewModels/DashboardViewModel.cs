namespace Bank_System.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalBankBalance { get; set; }
        public decimal TotalDepositedAmount { get; set; }
        public decimal TotalWithdrawnAmount { get; set; }
    }
}
