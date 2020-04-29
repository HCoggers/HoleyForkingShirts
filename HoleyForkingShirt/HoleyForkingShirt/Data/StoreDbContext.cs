using HoleyForkingShirt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }
        /// <summary>
        /// This is all the data that is seeded into our database for the products we are selling. 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    Sku = "500boots500",
                    Name = "Hiking Boots",
                    Description = "The sole has split in the front, but you could probably fix it with a couple nails?",
                    Price = 15.00M,
                    Size = Sizes.L,
                    Image = "https://torange.biz/photofx/180/8/light-grey-fuzzy-border-torn-old-shoe-180924.jpg"
                },
                new Product
                {
                    ID = 2,
                    Sku = "42jeanset42",
                    Name = "Skinny Jeans Set",
                    Description = "A four-in-one set of our latest trashy skinny jeans.",
                    Price = 25.00M,
                    Size = Sizes.S,
                    Image = "https://cdn.pixabay.com/photo/2019/10/27/14/34/jeans-4581931_960_720.jpg"
                },
                new Product
                {
                    ID = 3,
                    Sku = "670Patchy670",
                    Name = "Patched Up Hipster Mom Jeans",
                    Description = "These jeans are our best in stock! only two holes",
                    Price = 39.00M,
                    Size = Sizes.M,
                    Image = "https://haitao.nos.netease.com/20190108012559f80a180239864203a738ba1c7269aed6.jpeg?klsize=1534x2301"
                },
                new Product
                {
                    ID = 4,
                    Sku = "85StretchySweat85",
                    Name = "Sweatshirt With Split Seams",
                    Description = "This one's a fixer-upper! Break out your Singer 4452 and you'll be nice and cozy this winter",
                    Price = 12.00M,
                    Size = Sizes.XXL,
                    Image = "https://storage.needpix.com/rsynced_images/clothes-2063789_1280.jpg"
                },
                new Product
                {
                    ID = 5,
                    Sku = "123Mothy123",
                    Name = "Grey Moth-Eaten Tee-shirt",
                    Description = "How else are you going to show off your sick tats than with a bunch of holes in your shirt?",
                    Price = 18.00M,
                    Size = Sizes.L,
                    Image = "https://c1.wallpaperflare.com/preview/114/131/759/fashion-vintage-distressed-store.jpg"
                },
                new Product
                {
                    ID = 6,
                    Sku = "789GreenWoodsman789",
                    Name = "Green Flannel Shirt",
                    Description = "The cuffs on this lumberjack's dream are scuffed and buttonless, but hey, just roll 'em up!",
                    Price = 23.00M,
                    Size = Sizes.XL,
                    Image = "https://i.imgur.com/2beVsrR.jpg"
                },
                new Product
                {
                    ID = 7,
                    Sku = "super80super",
                    Name = "Super Torn Up Denim Jacket",
                    Description = "Show off your undershirt with this absolutely DESTROYED denim jacket",
                    Price = 13.99M,
                    Size = Sizes.M,
                    Image = "https://imgur.com/C6F0b17.jpg"
                },
                new Product
                {
                    ID = 8,
                    Sku = "27darn27",
                    Name = "Holey Blue Socks",
                    Description = "Give that right big toe some breathing room with these un-darned socks!",
                    Price = 7.00M,
                    Size = Sizes.XXL,
                    Image = "https://imgur.com/qFObFkC.jpg"
                },
                new Product
                {
                    ID = 9,
                    Sku = "1800stretch1800",
                    Name = "A verrrrry Stretched Black Sweater",
                    Description = "The tag says XS, but we both know that's no longer true.",
                    Price = 9.50M,
                    Size = Sizes.L,
                    Image = "https://imgur.com/GOTgNnd.jpg"
                },
                new Product
                {
                    ID = 10,
                    Sku = "86ROCK86",
                    Name = "Slashed Rocker Tee",
                    Description = "This one might actually be intentional. This shirt looks perfect for a budding (starving) musician.",
                    Price = 15.00M,
                    Size = Sizes.S,
                    Image = "https://imgur.com/XNFTeA1.jpg"
                }
                );

            modelBuilder.Entity<CartItems>().HasKey(c => new { c.CartID, c.ProductID });
        }
                
        /// <summary>
        /// This is connecting our cart to our database. 
        /// </summary>
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
    }
}
