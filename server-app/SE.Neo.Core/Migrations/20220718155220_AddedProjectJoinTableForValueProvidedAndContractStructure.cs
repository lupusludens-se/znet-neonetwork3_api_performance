using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedProjectJoinTableForValueProvidedAndContractStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project_Contract_Structure",
                columns: table => new
                {
                    Project_Contract_Structure_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Contract_Structure", x => x.Project_Contract_Structure_Id);
                    table.ForeignKey(
                        name: "FK_Project_Contract_Structure_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Contract_Structure_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Value_Provided",
                columns: table => new
                {
                    Project_Value_Provided_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Value_Provided", x => x.Project_Value_Provided_Id);
                    table.ForeignKey(
                        name: "FK_Project_Value_Provided_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Value_Provided_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Contract_Structure_Contract_Structure_Id",
                table: "Project_Contract_Structure",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Contract_Structure_Project_Id_Contract_Structure_Id",
                table: "Project_Contract_Structure",
                columns: new[] { "Project_Id", "Contract_Structure_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Value_Provided_Project_Id_Value_Provided_Id",
                table: "Project_Value_Provided",
                columns: new[] { "Project_Id", "Value_Provided_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Value_Provided_Value_Provided_Id",
                table: "Project_Value_Provided",
                column: "Value_Provided_Id");

            migrationBuilder.Sql(@"
                INSERT INTO [Project_Value_Provided] ([Project_Id], [Value_Provided_Id])
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Renewable_Retail_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Onsite_Solar_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Green_Tariffs_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Fuel_Cells_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_EV_Charging_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Emerging_Technology_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Efficiency_Equipment_Measures_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Efficiency_Audits_And_Consulting_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_EAC_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Community_Solar_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Carbon_Offsets_Details] WHERE [Value_Provided_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Value_Provided_Id] FROM [Project_Battery_Storage_Details] WHERE [Value_Provided_Id] IS NOT NULL
  
                INSERT INTO [Project_Contract_Structure] ([Project_Id], [Contract_Structure_Id])
                    SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Onsite_Solar_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Fuel_Cells_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_EV_Charging_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Emerging_Technology_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Efficiency_Equipment_Measures_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Efficiency_Audits_And_Consulting_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Community_Solar_Details] WHERE [Contract_Structure_Id] IS NOT NULL
	                UNION
	                SELECT [Project_Id], [Contract_Structure_Id] FROM [Project_Battery_Storage_Details] WHERE [Contract_Structure_Id] IS NOT NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Battery_Storage_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Battery_Storage_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Carbon_Offsets_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Community_Solar_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Community_Solar_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_EAC_Details_Value_Provided_Value_Provided_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Emerging_Technology_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Emerging_Technology_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_EV_Charging_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_EV_Charging_Details_Value_Provided_Value_Provided_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Green_Tariffs_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Onsite_Solar_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Onsite_Solar_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Renewable_Retail_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Value_Provided_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Value_Provided_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Green_Tariffs_Details_Value_Provided_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Contract_Structure_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Value_Provided_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Value_Provided_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Value_Provided_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Contract_Structure_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Value_Provided_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Contract_Structure_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Value_Provided_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropColumn(
                name: "Contract_Structure_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropColumn(
                name: "Value_Provided_Id",
                table: "Project_Battery_Storage_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project_Contract_Structure");

            migrationBuilder.DropTable(
                name: "Project_Value_Provided");

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Renewable_Retail_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Onsite_Solar_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Green_Tariffs_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_EV_Charging_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_EV_Charging_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Emerging_Technology_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_EAC_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Community_Solar_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Community_Solar_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contract_Structure_Id",
                table: "Project_Battery_Storage_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Value_Provided_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Value_Provided_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Value_Provided_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Contract_Structure_Id",
                table: "Project_EV_Charging_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Value_Provided_Id",
                table: "Project_EV_Charging_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Value_Provided_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Value_Provided_Id",
                table: "Project_EAC_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Contract_Structure_Id",
                table: "Project_Community_Solar_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Value_Provided_Id",
                table: "Project_Community_Solar_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Contract_Structure_Id",
                table: "Project_Battery_Storage_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                column: "Value_Provided_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Battery_Storage_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Battery_Storage_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Battery_Storage_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Carbon_Offsets_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Community_Solar_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Community_Solar_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Community_Solar_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Community_Solar_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_EAC_Details_Value_Provided_Value_Provided_Id",
                table: "Project_EAC_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Emerging_Technology_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Emerging_Technology_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_EV_Charging_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_EV_Charging_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_EV_Charging_Details_Value_Provided_Value_Provided_Id",
                table: "Project_EV_Charging_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Green_Tariffs_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Onsite_Solar_Details_Contract_Structure_Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Onsite_Solar_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Renewable_Retail_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
