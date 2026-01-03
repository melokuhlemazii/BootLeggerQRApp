using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BootLeggerQRApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table : "MenuItems",
                columns : new[] { "Name", "Description", "Price", "Category", "IsAvailable", "ImagePath" },
                values : new object[,]
                
                {
                    { "Cheeseburger", "Juicy beef burger with cheese", 79.99m, "Food", true, "/images/cheeseburgerr.jpg" },
                    { "Margherita Pizza", "Classic pizza with tomato & cheese", 99.50m, "Food", true, "/images/pizza.jpg" },
                    { "Coca Cola", "500ml bottle", 20.00m, "Drinks", true, "/images/coke.jpg" }
                });
       
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
