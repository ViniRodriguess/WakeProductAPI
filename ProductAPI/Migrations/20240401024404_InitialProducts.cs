using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Products",
            columns: ["Id", "Name", "Stock", "Price"],
            values: new object[,]
            {
                { Guid.NewGuid(), "Produto 1", 10, 20.5m },
                { Guid.NewGuid(), "Produto 2", 15, 30.75m },
                { Guid.NewGuid(), "Produto 3", 20, 40.25m },
                { Guid.NewGuid(), "Produto 4", 25, 50.0m },
                { Guid.NewGuid(), "Produto 5", 30, 60.5m }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
