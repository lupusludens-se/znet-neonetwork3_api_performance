using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateColumnNameScaleIdInRegionScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("InitiativeScaleId", "Region_Scale_Initiative", "Scale_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Scale_Id", "Region_Scale_Initiative", "InitiativeScaleId");
        }
    }
}
