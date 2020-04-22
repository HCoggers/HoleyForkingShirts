using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HFShirtsTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanNameProduct()
        {
            Product product = new Product();
            product.Name = "Lensless Glasses";

            Assert.Equal("Lensless Glasses", product.Name);
        }

        [Fact]
        public void CanSetDescriptionOfProduct()
        {
            Product product = new Product
            {
                Name = "Mismatched Socks",
                Description = "The Left one is red, and XL, the Right one is blue and S"
            };

            Assert.Equal("The Left one is red, and XL, the Right one is blue and S", product.Description);
        }

        [Fact]
        public void CanSetSizeOfProduct()
        {
            Product product = new Product
            {
                Name = "Mismatched Socks",
                Size = Sizes.M
            };

            Assert.Equal(Sizes.M, product.Size);
        }

        [Fact]
        public void CanSetPriceOfProduct()
        {
            Product product = new Product
            {
                Name = "Mismatched Socks",
                Price = 56.00M
            };

            Assert.Equal(56.00M, product.Price);
        }

        [Fact]
        public async Task CanAddProductToDb()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("TestDb1")
                .Options;

            using (StoreDbContext context = new StoreDbContext(options))
            {
                InventoryService service = new InventoryService(context);

                Product product = new Product()
                {
                    ID = 7,
                    Name = "Mismatched Socks",
                    Sku = "69xls69",
                    Description = "The Left one is red, and XL, the Right one is blue and S",
                    Size = Sizes.M,
                    Price = 56.99M,
                    Image = "someimage.jpg"
                };
                
                var result = await service.CreateProduct(product);

                var data = context.Products.Find(product.ID);
                Assert.Equal(result, data);
            }

        }

        [Fact]
        public async Task CanGetProductFromDb()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("TestDb2")
                .Options;

            using StoreDbContext context = new StoreDbContext(options);
            InventoryService service = new InventoryService(context);

            var result = await service.CreateProduct(new Product()
            {
                ID = 3,
                Name = "Broken Glasses",
                Sku = "20nomore20",
                Description = "One lens is missing, the other is a half inch thick.",
                Size = Sizes.L,
                Price = 20.40M,
                Image = "picture.jpg"
            });

            Assert.Equal(result, await service.GetProductById(result.ID));

        }

        [Fact]
        public async Task CanGetAllProducts()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("TestDb3")
                .Options;

            using StoreDbContext context = new StoreDbContext(options);
            InventoryService service = new InventoryService(context);

            var products = new List<Product>
            {
                new Product()
                {
                    ID = 3,
                    Name = "Broken Glasses",
                    Sku = "20nomore20",
                    Description = "One lens is missing, the other is a half inch thick.",
                    Size = Sizes.L,
                    Price = 20.40M,
                    Image = "picture.jpg"
                },
                new Product()
                {
                    ID = 7,
                    Name = "Mismatched Socks",
                    Sku = "69xls69",
                    Description = "The Left one is red, and XL, the Right one is blue and S",
                    Size = Sizes.M,
                    Price = 56.99M,
                    Image = "someimage.jpg"
                },
                new Product()
                {
                    ID = 14,
                    Name = "My favorite Cardigan",
                    Sku = "33somanyholes33",
                    Description = "My mom made me donate this, even though the hole is barely the size of a softball!",
                    Size = Sizes.S,
                    Price = 32.99M,
                    Image = "cardigan.jpg"
                }
            };

            foreach (var p in products)
                await service.CreateProduct(p);

            Assert.Equal(products, await service.GetProducts());

        }

        [Fact]
        public async Task CanUpdateProduct()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("TestDb4")
                .Options;

            using StoreDbContext context = new StoreDbContext(options);
            InventoryService service = new InventoryService(context);

            var product = new Product()
            {
                ID = 14,
                Name = "My favorite Cardigan",
                Sku = "33somanyholes33",
                Description = "My mom made me donate this, even though the hole is barely the size of a softball!",
                Size = Sizes.S,
                Price = 32.99M,
                Image = "cardigan.jpg"
            };
            await service.CreateProduct(product);

            product.Price = 46.99M;

            await service.UpdateProduct(product.ID, product);
            decimal upsell = service.GetProductById(14).Result.Price;

            Assert.Equal(46.99M, upsell);
        }

        [Fact]
        public async Task CanDeleteProduct()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("TestDb5")
                .Options;

            using StoreDbContext context = new StoreDbContext(options);
            InventoryService service = new InventoryService(context);

            var product = new Product()
            {
                ID = 14,
                Name = "My favorite Cardigan",
                Sku = "33somanyholes33",
                Description = "My mom made me donate this, even though the hole is barely the size of a softball!",
                Size = Sizes.S,
                Price = 32.99M,
                Image = "cardigan.jpg"
            };
            await service.CreateProduct(product);
            await service.DeleteProduct(product.ID);

            Assert.Null(await service.GetProductById(product.ID));
        }

    }
}
