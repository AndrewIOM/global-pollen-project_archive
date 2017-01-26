using Microsoft.EntityFrameworkCore.Migrations;

namespace GlobalPollenProject.Infrastructure.Migrations
{
    public partial class SlidesNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_ReferenceSlide_ReferenceSlideId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceSlide_ReferenceCollections_BelongsToId",
                table: "ReferenceSlide");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceSlide_Taxa_TaxonId",
                table: "ReferenceSlide");

            migrationBuilder.DropForeignKey(
                name: "FK_UnknownGrains_Taxa_TaxonId",
                table: "UnknownGrains");

            migrationBuilder.DropIndex(
                name: "IX_UnknownGrains_TaxonId",
                table: "UnknownGrains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReferenceSlide",
                table: "ReferenceSlide");

            migrationBuilder.DropColumn(
                name: "TaxonId",
                table: "UnknownGrains");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slides",
                table: "ReferenceSlide",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Slides_ReferenceSlideId",
                table: "Image",
                column: "ReferenceSlideId",
                principalTable: "ReferenceSlide",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_ReferenceCollections_BelongsToId",
                table: "ReferenceSlide",
                column: "BelongsToId",
                principalTable: "ReferenceCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Taxa_TaxonId",
                table: "ReferenceSlide",
                column: "TaxonId",
                principalTable: "Taxa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceSlide_TaxonId",
                table: "ReferenceSlide",
                newName: "IX_Slides_TaxonId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceSlide_BelongsToId",
                table: "ReferenceSlide",
                newName: "IX_Slides_BelongsToId");

            migrationBuilder.RenameTable(
                name: "ReferenceSlide",
                newName: "Slides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Slides_ReferenceSlideId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_ReferenceCollections_BelongsToId",
                table: "Slides");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Taxa_TaxonId",
                table: "Slides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slides",
                table: "Slides");

            migrationBuilder.AddColumn<int>(
                name: "TaxonId",
                table: "UnknownGrains",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnknownGrains_TaxonId",
                table: "UnknownGrains",
                column: "TaxonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReferenceSlide",
                table: "Slides",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_ReferenceSlide_ReferenceSlideId",
                table: "Image",
                column: "ReferenceSlideId",
                principalTable: "Slides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceSlide_ReferenceCollections_BelongsToId",
                table: "Slides",
                column: "BelongsToId",
                principalTable: "ReferenceCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceSlide_Taxa_TaxonId",
                table: "Slides",
                column: "TaxonId",
                principalTable: "Taxa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnknownGrains_Taxa_TaxonId",
                table: "UnknownGrains",
                column: "TaxonId",
                principalTable: "Taxa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Slides_TaxonId",
                table: "Slides",
                newName: "IX_ReferenceSlide_TaxonId");

            migrationBuilder.RenameIndex(
                name: "IX_Slides_BelongsToId",
                table: "Slides",
                newName: "IX_ReferenceSlide_BelongsToId");

            migrationBuilder.RenameTable(
                name: "Slides",
                newName: "ReferenceSlide");
        }
    }
}
