using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedProjectOffsitePPADetailsColumnsType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discount_Amount_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE Project_Offsite_Power_Purchase_Agreement_Details
                SET Discount_Amount_Temp =
                    CASE WHEN Discount_Amount IS NULL THEN ''
                        ELSE CAST(Discount_Amount AS nvarchar(100))
                    END");

            migrationBuilder.DropColumn(
                name: "Discount_Amount",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.RenameColumn(
                name: "Discount_Amount_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                newName: "Discount_Amount");

            migrationBuilder.AddColumn<string>(
                name: "Contract_Price_Per_MWh_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE Project_Offsite_Power_Purchase_Agreement_Details
                SET Contract_Price_Per_MWh_Temp =
                    CASE WHEN Contract_Price_Per_MWh IS NULL THEN ''
                        ELSE CAST(Contract_Price_Per_MWh AS nvarchar(100))
                    END");

            migrationBuilder.DropColumn(
                name: "Contract_Price_Per_MWh",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.RenameColumn(
                name: "Contract_Price_Per_MWh_Temp",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                newName: "Contract_Price_Per_MWh");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Discount_Amount",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Contract_Price_Per_MWh",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);
        }
    }
}
