using Bank_System.Models;
using Bank_System.ViewModels;
using BankSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankDbContext _context;

        public HomeController(BankDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardModel = new DashboardViewModel
            {
                TotalClients = await _context.Clients.CountAsync(),
                TotalAccounts = await _context.BankAccounts.CountAsync(),
                TotalTransactions = await _context.Transactions.CountAsync(),
                TotalBankBalance = await _context.BankAccounts.SumAsync(a => (decimal?)a.Balance) ?? 0,
                TotalDepositedAmount = await _context.Transactions
                    .Where(t => t.Type == TransactionType.Deposit)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0,
                TotalWithdrawnAmount = await _context.Transactions
                    .Where(t => t.Type == TransactionType.Withdrawal)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0
            };

            return View(dashboardModel);
        }
    }
}