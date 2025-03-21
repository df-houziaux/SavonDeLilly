using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SavonDeLilly.Data;
using SavonDeLilly.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SavonDeLilly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ajouter les services à la conteneur
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configuration des services d'identité
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configurations des options d'identité
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.SignIn.RequireConfirmedAccount = false; // Modifier selon vos besoins
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configuration de l'authentification par cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/MonCompte/Login"; // Page de connexion
                    options.LogoutPath = "/MonCompte/Logout"; // Page de déconnexion
                    options.AccessDeniedPath = "/Home/AccessDenied"; // Page d'accès refusé
                });

            // Ajouter la gestion des sessions
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add controllers and views
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<MailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Activer la gestion des sessions
            app.UseSession();
            app.UseAuthentication(); // Activer l'authentification
            app.UseAuthorization();  // Activer l'autorisation

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
