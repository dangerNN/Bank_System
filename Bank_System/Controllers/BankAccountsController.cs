using Bank_System.Models;
using Bank_System.ViewModels;
using BankSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly BankDbContext _context;

        public BankAccountsController(BankDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.BankAccounts.Include(a => a.Client).ToListAsync();
            return View(accounts);
        }
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BankAccount account)
        {
            if (ModelState.IsValid)
            {
                account.AccountNumber = "UA" + Random.Shared.Next(10000000, 99999999).ToString();
                account.Balance = 0;

                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", account.ClientId);
            return View(account);
        }
        public async Task<IActionResult> Details(int id, DateTime? searchDate, TransactionType? filterType)
        {
            var account = await _context.BankAccounts
                .Include(a => a.Client)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            var query = _context.Transactions
                .Where(t => t.BankAccountId == id)
                .AsQueryable();

            if (searchDate.HasValue)
            {
                query = query.Where(t => t.Date.Date == searchDate.Value.Date);
            }

            if (filterType.HasValue)
            {
                query = query.Where(t => t.Type == filterType.Value);
            }

            var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();

            var allTransactions = account.Transactions;

            var viewModel = new AccountDetailsViewModel
            {
                Account = account,
                Transactions = transactions,
                SearchDate = searchDate,
                FilterType = filterType,
                TotalTransactionsCount = allTransactions.Count,
                TotalDepositsSum = allTransactions.Where(t => t.Type == TransactionType.Deposit).Sum(t => t.Amount),
                TotalWithdrawalsSum = allTransactions.Where(t => t.Type == TransactionType.Withdrawal).Sum(t => t.Amount)
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Deposit(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account == null) return NotFound();

            ViewBag.Account = account;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int id, decimal amount, string description)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account == null) return NotFound();
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Сума поповнення повинна бути більшою за нуль.");
                ViewBag.Account = account;
                return View();
            }

            account.Balance += amount;

            var transaction = new Transaction
            {
                BankAccountId = account.Id,
                Amount = amount,
                Type = TransactionType.Deposit,
                Date = DateTime.Now,
                Description = string.IsNullOrWhiteSpace(description) ? "Поповнення рахунку" : description
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = account.Id });
        }
        public async Task<IActionResult> Withdraw(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account == null) return NotFound();

            ViewBag.Account = account;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id, decimal amount, string description)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account == null) return NotFound();
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Сума зняття повинна бути більшою за нуль.");
                ViewBag.Account = account;
                return View();
            }
            if (amount > account.Balance)
            {
                ModelState.AddModelError("", "Недостатньо коштів на рахунку для здійснення цієї операції.");
                ViewBag.Account = account;
                return View();
            }
            account.Balance -= amount;
            var transaction = new Transaction
            {
                BankAccountId = account.Id,
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Date = DateTime.Now,
                Description = string.IsNullOrWhiteSpace(description) ? "Зняття коштів" : description
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = account.Id });
        }
    }
}