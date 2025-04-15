using ApplicationLayer.Features.Products.Commands;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Interfaces.Identity;
using ApplicationLayer.Interfaces.Infrastructure;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using ApplicationLayer.Services;
using Domain.Entities;
using Infrastructure.ApiServices;
using Infrastructure.DbContext;
using Infrastructure.DbContext.DataSeed;
using Infrastructure.Identity.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SistemaPOS.Services;
using SistemaPOS.Views;

namespace SistemaPOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddDbContext<PosDbContext>();
            SQLitePCL.Batteries.Init();
            builder.Services.TryAddScoped<IIdentityService, IdentityService>();
            builder.Services.TryAddTransient<DbSeeder>();
            builder.Services.TryAddTransient<IDbSeeder, DbSeeder>();
            builder.Services.TryAddScoped<IProductApiService, ProductApiService>();
            builder.Services.TryAddScoped<ICustomerApiService, CustomerApiService>();
            builder.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.TryAddSingleton<ICartService, InMemoryCartService>();
            builder.Services.TryAddSingleton<IAuthenticatedUserService, AuthenticatedUserService>();
            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<CheckoutPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveProductsCommand).Assembly));
            builder.Services.AddHttpClient("CustomersApiClient", client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddHttpClient("ProductsApiClient", client =>
            {
                client.BaseAddress = new Uri("https://fakestoreapi.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            }).AddEntityFrameworkStores<PosDbContext>().AddDefaultTokenProviders();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app = builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).Build();

            // Use a local variable to store the services
            var services = app.Services;
            App.Services = app.Services;

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PosDbContext>();
            db.Database.EnsureCreated();
            var dbSeeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
            dbSeeder.SeedAsync().Wait();
            return app;
        }
    }
}
