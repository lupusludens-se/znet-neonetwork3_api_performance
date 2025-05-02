using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddTableRegionScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Region_Scale_Initiative",
                columns: table => new
                {
                    Region_Scale_Initiative_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InitiativeScaleId = table.Column<int>(type: "int", nullable: false),
                    CMS_Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region_Scale_Initiative", x => x.Region_Scale_Initiative_Id);
                    table.ForeignKey(
                        name: "FK_Region_Scale_Initiative_CMS_Region_CMS_Region_Id",
                        column: x => x.CMS_Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Region_Scale_Initiative_Initiative_Scale_InitiativeScaleId",
                        column: x => x.InitiativeScaleId,
                        principalTable: "Initiative_Scale",
                        principalColumn: "Initiative_Scale_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Region_Scale_Initiative_CMS_Region_Id",
                table: "Region_Scale_Initiative",
                column: "CMS_Region_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Region_Scale_Initiative_InitiativeScaleId",
                table: "Region_Scale_Initiative",
                column: "InitiativeScaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Region_Scale_Initiative");
        }
    }
}
