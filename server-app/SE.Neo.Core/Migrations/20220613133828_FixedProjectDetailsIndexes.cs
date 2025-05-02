using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedProjectDetailsIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_Technology_Project_Id",
                table: "Project_Technology");

            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option");

            migrationBuilder.DropIndex(
                name: "IX_Project_Region_Project_Id",
                table: "Project_Region");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Term_Length_Project_EAC_Details_Id",
                table: "Project_EAC_Details_Term_Length");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Technology_Project_Id_Technology_Id",
                table: "Project_Technology",
                columns: new[] { "Project_Id", "Technology_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Id_Purchase_Option_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option",
                columns: new[] { "Project_Renewable_Retail_Details_Id", "Purchase_Option_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Region_Project_Id_Region_Id",
                table: "Project_Region",
                columns: new[] { "Project_Id", "Region_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Id_Value_Pr~",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided",
                columns: new[] { "Project_Offsite_Power_Purchase_Agreement_Details_Id", "Value_Provided_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Term_Length_Project_EAC_Details_Id_Term_Length_Id",
                table: "Project_EAC_Details_Term_Length",
                columns: new[] { "Project_EAC_Details_Id", "Term_Length_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Id_Term_Length_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length",
                columns: new[] { "Project_Carbon_Offsets_Details_Id", "Term_Length_Id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_Technology_Project_Id_Technology_Id",
                table: "Project_Technology");

            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Id_Purchase_Option_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option");

            migrationBuilder.DropIndex(
                name: "IX_Project_Region_Project_Id_Region_Id",
                table: "Project_Region");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Id_Value_Pr~",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Term_Length_Project_EAC_Details_Id_Term_Length_Id",
                table: "Project_EAC_Details_Term_Length");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Id_Term_Length_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Technology_Project_Id",
                table: "Project_Technology",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option",
                column: "Project_Renewable_Retail_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Region_Project_Id",
                table: "Project_Region",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided",
                column: "Project_Offsite_Power_Purchase_Agreement_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Term_Length_Project_EAC_Details_Id",
                table: "Project_EAC_Details_Term_Length",
                column: "Project_EAC_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length",
                column: "Project_Carbon_Offsets_Details_Id");
        }
    }
}
