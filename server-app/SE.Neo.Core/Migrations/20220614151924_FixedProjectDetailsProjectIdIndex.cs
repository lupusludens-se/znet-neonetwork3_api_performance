using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedProjectDetailsProjectIdIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details",
                column: "Project_Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details",
                column: "Project_Id");
        }
    }
}
