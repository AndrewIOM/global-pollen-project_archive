using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace OxPollen.Migrations
{
    public partial class debugchanges : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.DropColumn(name: "DateOfIdentification", table: "Identification");
            migration.DropColumn(name: "GbifId", table: "Identification");
            migration.AddColumn(
                name: "TimeIdentified",
                table: "Identification",
                type: "datetime2",
                nullable: false);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropColumn(name: "TimeIdentified", table: "Identification");
            migration.AddColumn(
                name: "DateOfIdentification",
                table: "Identification",
                type: "datetime2",
                nullable: false);
            migration.AddColumn(
                name: "GbifId",
                table: "Identification",
                type: "int",
                nullable: false);
        }
    }
}
