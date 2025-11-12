using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productsCollection)
        {
            var filter = Builders<Product>.Filter.Empty;
            // If there are no documents, insert the preconfigured products
            if (productsCollection.CountDocuments(filter) == 0)
            {
                productsCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Logitech Wireless Mouse",
                    Category = "Electronics",
                    Summary = "Compact wireless mouse with long battery life.",
                    Description = "Ergonomic wireless mouse with adjustable DPI and 24-month battery life.",
                    ImageFile = "mouse.png",
                    Price = 29.99m
                },
                new Product
                {   Id = "602d2149e773f2a3990b47f6",
                    Name = "Noise-Cancelling Headphones",
                    Category = "Audio",
                    Summary = "Over-ear noise-cancelling headphones.",
                    Description = "High-fidelity sound, active noise cancellation, comfortable ear cushions for long listening sessions.",
                    ImageFile = "headphones.png",
                    Price = 199.99m
                },
                new Product
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "USB-C Charging Cable",
                    Category = "Accessories",
                    Summary = "Durable braided USB-C cable.",
                    Description = "Fast charging and data transfer cable with reinforced connectors and nylon braid.",
                    ImageFile = "cable.png",
                    Price = 9.99m
                },
                new Product
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "4K Monitor 27\"",
                    Category = "Computers",
                    Summary = "27-inch 4K UHD monitor with HDR.",
                    Description = "Ultra-high-definition display with HDR support, thin bezels, and adjustable stand.",
                    ImageFile = "monitor.png",
                    Price = 349.99m
                },
                new Product
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "Mechanical Keyboard",
                    Category = "Peripherals",
                    Summary = "Tactile mechanical keyboard with RGB.",
                    Description = "Durable mechanical switches, customizable RGB lighting, and programmable macros.",
                    ImageFile = "keyboard.png",
                    Price = 129.50m
                }
            };
        }
    }
}
