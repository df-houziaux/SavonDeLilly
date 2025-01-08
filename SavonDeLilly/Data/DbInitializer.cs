using SavonDeLilly.Models;
using Microsoft.EntityFrameworkCore;

namespace SavonDeLilly.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();


            context.Database.EnsureCreated();

            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Products");


            if (context.Products.Any())
            {
                return;
            }

            var products = new[]
            {
                // Savons Lavande
                new Product { Name = "Savon Lavande 1", ImageUrl = "/images/savonlavande1.jpg", Description = "Savon parfumé à la Lavande", Price = 15.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 10 },
                new Product { Name = "Savon Lavande 2", ImageUrl = "/images/savonlavande2.jpg", Description = "Savon parfumé à la Lavande", Price = 16.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 0 },
                new Product { Name = "Savon Lavande 3", ImageUrl = "/images/savonlavande3.jpg", Description = "Savon parfumé à la Lavande", Price = 17.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 5 },
                new Product { Name = "savon Lavande 4", ImageUrl = "/images/savonlavande4.jpg", Description = "savon parfumée à la Lavande", Price = 18.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 6 },
                new Product { Name = "savon Lavande 5", ImageUrl = "/images/savonlavande5.jpg", Description = "savon parfumée à la Lavande", Price = 19.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 8 },
                new Product { Name = "savon Lavande 6", ImageUrl = "/images/savonlavande6.jpg", Description = "savon parfumée à la Lavande", Price = 20.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 5 },
                new Product { Name = "savon Lavande 7", ImageUrl = "/images/savonlavande7.jpg", Description = "savon parfumée à la Lavande", Price = 21.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 0 },
                new Product { Name = "savon Lavande 8", ImageUrl = "/images/savonlavande8.jpg", Description = "savon parfumée à la Lavande", Price = 22.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 4 },
                new Product { Name = "savon Lavande 9", ImageUrl = "/images/savonlavande9.jpg", Description = "savon parfumée à la Lavande", Price = 23.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 69 },
                new Product { Name = "savon Lavande 10", ImageUrl = "/images/savonlavande10.jpg", Description = "savon parfumée à la Lavande", Price = 24.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 42 },
                new Product { Name = "savon Lavande 11", ImageUrl = "/images/savonlavande11.jpg", Description = "savon parfumée à la Lavande", Price = 25.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 26 },
                new Product { Name = "savon Lavande 12", ImageUrl = "/images/savonlavande12.jpg", Description = "savon parfumée à la Lavande", Price = 26.99m, Ingredients = "Lavande, Cire de soja", Category = "Catégorie 1", Stock = 18 },

                // Savons Vanille
                new Product { Name = "Savon Vanille 1", ImageUrl = "/images/savonvanille1.jpg", Description = "Savon parfumé à la Vanille", Price = 15.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 8 },
                new Product { Name = "Savon Vanille 2", ImageUrl = "/images/savonvanille2.jpg", Description = "Savon parfumé à la Vanille", Price = 16.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 0 },
                new Product { Name = "Savon Vanille 3", ImageUrl = "/images/savonvanille3.jpg", Description = "Savon parfumé à la Vanille", Price = 17.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 12 },
                new Product { Name = "savon Vanille 4", ImageUrl = "/images/savonvanille4.jpg", Description = "savon parfumée à la vanille ", Price = 18.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 5", ImageUrl = "/images/savonvanille5.jpg", Description = "savon parfumée à la vanille ", Price = 19.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 6", ImageUrl = "/images/savonvanille6.jpg", Description = "savon parfumée à la vanille ", Price = 20.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 7", ImageUrl = "/images/savonvanille7.jpg", Description = "savon parfumée à la vanille ", Price = 21.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 8", ImageUrl = "/images/savonvanille8.jpg", Description = "savon parfumée à la vanille ", Price = 22.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 9", ImageUrl = "/images/savonvanille9.jpg", Description = "savon parfumée à la vanille ", Price = 23.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 10", ImageUrl = "/images/savonvanille10.jpg", Description = "savon parfumée à la vanille ", Price = 24.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 11", ImageUrl = "/images/savonvanille11.jpg", Description = "savon parfumée à la vanille ", Price = 25.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },
                new Product { Name = "savon Vanille 12", ImageUrl = "/images/savonvanille12.jpg", Description = "savon parfumée à la vanille ", Price = 26.99m, Ingredients = "Vanille, Cire de soja", Category = "Catégorie 2", Stock = 5 },

                // Savons Rosée
                new Product { Name = "Savon Rosé 1", ImageUrl = "/images/savonrosée1.jpg", Description = "Savon parfumé à la rose", Price = 17.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 6 },
                new Product { Name = "Savon Rosé 2", ImageUrl = "/images/savonrosée2.jpg", Description = "Savon parfumé à la rose", Price = 18.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 7 },
                new Product { Name = "Savon Rosé 3", ImageUrl = "/images/savonrosée3.jpg", Description = "Savon parfumé à la rose", Price = 19.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 0 },
                new Product { Name = "savon Rosé 4", ImageUrl = "/images/savonrosée4.jpg", Description = "savon parfumée à la rose", Price = 20.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 1 },
                new Product { Name = "savon Rosé 5", ImageUrl = "/images/savonrosée5.jpg", Description = "savon parfumée à la rose", Price = 21.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 3 },
                new Product { Name = "savon Rosé 6", ImageUrl = "/images/savonrosée6.jpg", Description = "savon parfumée à la rose", Price = 22.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 7", ImageUrl = "/images/savonrosée7.jpg", Description = "savon parfumée à la rose", Price = 23.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 8", ImageUrl = "/images/savonrosée8.jpg", Description = "savon parfumée à la rose", Price = 24.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 9", ImageUrl = "/images/savonrosée9.jpg", Description = "savon parfumée à la rose", Price = 25.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 10", ImageUrl = "/images/savonrosée10.jpg", Description = "savon parfumée à la rose", Price = 26.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 11", ImageUrl = "/images/savonrosée11.jpg", Description = "savon parfumée à la rose", Price = 27.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
                new Product { Name = "savon Rosé 12", ImageUrl = "/images/savonrosée12.jpg", Description = "savon parfumée à la rose", Price = 28.99m, Ingredients = "Rosée, Cire de soja", Category = "Catégorie 3", Stock = 5 },
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
