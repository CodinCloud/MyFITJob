using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFITJob.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoProjectStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_JobOffers_Status",
                table: "JobOffers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_JobOffers_Status",
                table: "JobOffers",
                sql: "\"Status\" IN ('new','applied','interviewing','offered','rejected','accepted')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_JobOffers_Status",
                table: "JobOffers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_JobOffers_Status",
                table: "JobOffers",
                sql: "\"Status\" IN ('new','saved','applied','interview_planned','interviewed','offer_received','accepted','rejected','archived')");
        }
    }
}
