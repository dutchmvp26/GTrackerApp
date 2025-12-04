using BLL.Interfaces;
using BLL.Services;
using DAL.Repositories;

namespace GTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddRazorPages();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // how long session lasts
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // required for GDPR compliance
            }); 

            // Get the connection string manually
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddScoped<IGameRepository>(provider =>
            new GameRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<IUserRepository>(provider =>
            new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<AuthService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}
