using Microsoft.EntityFrameworkCore;
using BankSystem.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        //builder.Services.AddDbContext<BankDbContext>(options =>
        //    options.UseInMemoryDatabase("BankDb"));

        builder.Services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(@"Server=WIN-8G40ARFN4JP\DANGERNN;Database=Bank_Sys;Integrated Security=True;TrustServerCertificate=True;"));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}