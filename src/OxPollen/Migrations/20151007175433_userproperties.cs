using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace OxPollen.Migrations
{
    public partial class userproperties : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AddColumn(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            migration.AddColumn(
                name: "Institution",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            migration.AddColumn(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            migration.AddColumn(
                name: "Title",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropColumn(name: "FirstName", table: "AspNetUsers");
            migration.DropColumn(name: "Institution", table: "AspNetUsers");
            migration.DropColumn(name: "LastName", table: "AspNetUsers");
            migration.DropColumn(name: "Title", table: "AspNetUsers");
        }
    }
}
