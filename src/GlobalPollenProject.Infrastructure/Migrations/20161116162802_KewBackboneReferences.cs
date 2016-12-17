using Microsoft.EntityFrameworkCore.Migrations;

namespace GlobalPollenProject.Infrastructure.Migrations
{
    public partial class KewBackboneReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "BackboneTaxa",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceUrl",
                table: "BackboneTaxa",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "BackboneTaxa");

            migrationBuilder.DropColumn(
                name: "ReferenceUrl",
                table: "BackboneTaxa");
        }
    }
}
