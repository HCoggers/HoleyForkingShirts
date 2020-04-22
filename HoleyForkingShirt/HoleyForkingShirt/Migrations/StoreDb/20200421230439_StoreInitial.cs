using Microsoft.EntityFrameworkCore.Migrations;

namespace HoleyForkingShirt.Migrations.StoreDb
{
    public partial class StoreInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sku = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Description", "Image", "Name", "Price", "Size", "Sku" },
                values: new object[,]
                {
                    { 1, "The sole has split in the front, but you could probably fix it with a couple nails?", "https://torange.biz/photofx/180/8/light-grey-fuzzy-border-torn-old-shoe-180924.jpg", "Hiking Boots", 15.00m, 3, "500boots500" },
                    { 2, "A four-in-one set of our latest trashy skinny jeans.", "https://cdn.pixabay.com/photo/2019/10/27/14/34/jeans-4581931_960_720.jpg", "Skinny Jeans Set", 25.00m, 1, "42jeanset42" },
                    { 3, "These jeans are our best in stock! only two holes", "https://haitao.nos.netease.com/20190108012559f80a180239864203a738ba1c7269aed6.jpeg?klsize=1534x2301", "Patched Up Hipster Mom Jeans", 39.00m, 2, "670Patchy670" },
                    { 4, "This one's a fixer-upper! Break out your Singer 4452 and you'll be nice and cozy this winter", "https://storage.needpix.com/rsynced_images/clothes-2063789_1280.jpg", "Sweatshirt With Split Seams", 12.00m, 5, "85StretchySweat85" },
                    { 5, "How else are you going to show off your sick tats than with a bunch of holes in your shirt?", "https://c1.wallpaperflare.com/preview/114/131/759/fashion-vintage-distressed-store.jpg", "Grey Moth-Eaten Tee-shirt", 18.00m, 3, "123Mothy123" },
                    { 6, "The cuffs on this lumberjack's dream are scuffed and buttonless, but hey, just roll 'em up!", "https://i.imgur.com/2beVsrR.jpg", "Green Flannel Shirt", 23.00m, 4, "789GreenWoodsman789" },
                    { 7, "Show off your undershirt with this absolutely DESTROYED denim jacket", "https://imgur.com/C6F0b17.jpg", "Super Torn Up Denim Jacket", 13.99m, 2, "super80super" },
                    { 8, "Give that right big toe some breathing room with these un-darned socks!", "https://imgur.com/qFObFkC.jpg", "Holey Blue Socks", 7.00m, 5, "27darn27" },
                    { 9, "The tag says XS, but we both know that's no longer true.", "https://imgur.com/GOTgNnd.jpg", "A verrrrry Stretched Black Sweater", 9.50m, 3, "1800stretch1800" },
                    { 10, "This one might actually be intentional. This shirt looks perfect for a budding (starving) musician.", "https://imgur.com/XNFTeA1.jpg", "Slashed Rocker Tee", 15.00m, 1, "86ROCK86" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
