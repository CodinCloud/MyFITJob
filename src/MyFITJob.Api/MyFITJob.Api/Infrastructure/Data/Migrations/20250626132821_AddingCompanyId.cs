using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFITJob.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "JobOffers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "JobOffers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "JobOffers");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "JobOffers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
