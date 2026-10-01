using Bank_System.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>()
                .HasOne(a => a.Client)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.BankAccount)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.BankAccountId);
        }
    }
}