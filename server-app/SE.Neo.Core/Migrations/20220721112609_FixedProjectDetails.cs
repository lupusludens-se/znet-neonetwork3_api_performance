using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedProjectDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Minimum_Parking_Space",
                table: "Project_EV_Charging_Details",
                newName: "Minimum_Charging_Stations_Required");

            migrationBuilder.AddColumn<string>(
                name: "Project_MW_Currently_Available_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Project_Offsite_Power_Purchase_Agreement_Details
                SET Project_MW_Currently_Available_Temp =
                    CASE WHEN TRY_CAST(Project_MW_Currently_Available AS INT) IS NOT NULL THEN CAST(Project_MW_Currently_Available AS INT)
                        ELSE 1
                    END");

            migrationBuilder.DropColumn(
                name: "Project_MW_Currently_Available",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.RenameColumn(
                name: "Project_MW_Currently_Available_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                newName: "Project_MW_Currently_Available");

            migrationBuilder.AddColumn<string>(
                name: "EAC_Custom",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Settlement_Price_Interval_Custom",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EAC_Custom",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropColumn(
                name: "Settlement_Price_Interval_Custom",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.RenameColumn(
                name: "Minimum_Charging_Stations_Required",
                table: "Project_EV_Charging_Details",
                newName: "Minimum_Parking_Space");

            migrationBuilder.AlterColumn<string>(
                name: "Project_MW_Currently_Available",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
