using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedProjectDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Quote_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure");

            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Interest_Quote_Interest_Quote_Interest_Id",
                table: "Solar_Quote_Interest");

            migrationBuilder.DropTable(
                name: "Quote_Contract_Structure");

            migrationBuilder.DropTable(
                name: "Quote_Interest");

            migrationBuilder.RenameColumn(
                name: "Quote_Interest_Id",
                table: "Solar_Quote_Interest",
                newName: "Value_Provided_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Solar_Quote_Interest_Quote_Interest_Id",
                table: "Solar_Quote_Interest",
                newName: "IX_Solar_Quote_Interest_Value_Provided_Id");

            migrationBuilder.RenameColumn(
                name: "Quote_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                newName: "Contract_Structure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                newName: "IX_Solar_Quote_Contract_Structure_Contract_Structure_Id");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Pinned",
                table: "Project",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<string>(
                name: "Additional_Comments",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opportunity",
                table: "Project",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time_And_Urgency_Considerations",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contract_Structure",
                columns: table => new
                {
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: false),
                    Contract_Structure_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_Structure", x => x.Contract_Structure_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Value_Provided",
                columns: table => new
                {
                    Project_Value_Provided_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Value_Provided_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Value_Provided", x => x.Project_Value_Provided_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Battery_Storage_Details",
                columns: table => new
                {
                    Project_Battery_Storage_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual = table.Column<int>(type: "int", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Battery_Storage_Details", x => x.Project_Battery_Storage_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Battery_Storage_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Battery_Storage_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Battery_Storage_Details_Project_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Project_Value_Provided",
                        principalColumn: "Project_Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Fuel_Cells_Details",
                columns: table => new
                {
                    Project_Fuel_Cells_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual = table.Column<int>(type: "int", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Fuel_Cells_Details", x => x.Project_Fuel_Cells_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Fuel_Cells_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Fuel_Cells_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Fuel_Cells_Details_Project_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Project_Value_Provided",
                        principalColumn: "Project_Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Contract_Structure",
                columns: new[] { "Contract_Structure_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Contract_Structure_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "CashPurchase", null },
                    { 2, null, null, null, "PowerPurchaseAgreement", null },
                    { 3, null, null, null, "Other", null },
                    { 4, null, null, null, "PPA", null },
                    { 5, null, null, null, "Lease", null },
                    { 6, null, null, null, "SharedSavings", null },
                    { 7, null, null, null, "GuaranteedSavings", null }
                });

            migrationBuilder.InsertData(
                table: "Project_Value_Provided",
                columns: new[] { "Project_Value_Provided_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Project_Value_Provided_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "CostSavings", null },
                    { 2, null, null, null, "Environmental", null },
                    { 3, null, null, null, "Story", null },
                    { 4, null, null, null, "Resiliency", null },
                    { 5, null, null, null, "Other", null },
                    { 6, null, null, null, "RenewableAttributes", null },
                    { 7, null, null, null, "MitigatingClimateChange", null },
                    { 8, null, null, null, "CommunityBenefits", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Contract_Structure_Id",
                table: "Project_Battery_Storage_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Contract_Structure_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Value_Provided_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Contract_Structure_Contract_Structure_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                column: "Contract_Structure_Id",
                principalTable: "Contract_Structure",
                principalColumn: "Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Interest_Project_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest",
                column: "Value_Provided_Id",
                principalTable: "Project_Value_Provided",
                principalColumn: "Project_Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Contract_Structure_Contract_Structure_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure");

            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Interest_Project_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest");

            migrationBuilder.DropTable(
                name: "Project_Battery_Storage_Details");

            migrationBuilder.DropTable(
                name: "Project_Fuel_Cells_Details");

            migrationBuilder.DropTable(
                name: "Contract_Structure");

            migrationBuilder.DropTable(
                name: "Project_Value_Provided");

            migrationBuilder.DropColumn(
                name: "Additional_Comments",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Opportunity",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Time_And_Urgency_Considerations",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "Value_Provided_Id",
                table: "Solar_Quote_Interest",
                newName: "Quote_Interest_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Solar_Quote_Interest_Value_Provided_Id",
                table: "Solar_Quote_Interest",
                newName: "IX_Solar_Quote_Interest_Quote_Interest_Id");

            migrationBuilder.RenameColumn(
                name: "Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                newName: "Quote_Contract_Structure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Solar_Quote_Contract_Structure_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                newName: "IX_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Id");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Pinned",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Quote_Contract_Structure",
                columns: table => new
                {
                    Quote_Contract_Structure_Id = table.Column<int>(type: "int", nullable: false),
                    Quote_Contract_Structure_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote_Contract_Structure", x => x.Quote_Contract_Structure_Id);
                });

            migrationBuilder.CreateTable(
                name: "Quote_Interest",
                columns: table => new
                {
                    Quote_Interest_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quote_Interest_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote_Interest", x => x.Quote_Interest_Id);
                });

            migrationBuilder.InsertData(
                table: "Quote_Contract_Structure",
                columns: new[] { "Quote_Contract_Structure_Id", "Quote_Contract_Structure_Name" },
                values: new object[,]
                {
                    { 1, "Cash Purchase" },
                    { 2, "Power Purchase Agreement" },
                    { 3, "Others" }
                });

            migrationBuilder.InsertData(
                table: "Quote_Interest",
                columns: new[] { "Quote_Interest_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Quote_Interest_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Cost Savings", null },
                    { 2, null, null, null, "Environmental Attributes and/or Carbon Reduction Targets", null },
                    { 3, null, null, null, "Story / Publicity", null },
                    { 4, null, null, null, "Resiliency", null },
                    { 5, null, null, null, "Something Else", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Quote_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                column: "Quote_Contract_Structure_Id",
                principalTable: "Quote_Contract_Structure",
                principalColumn: "Quote_Contract_Structure_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Interest_Quote_Interest_Quote_Interest_Id",
                table: "Solar_Quote_Interest",
                column: "Quote_Interest_Id",
                principalTable: "Quote_Interest",
                principalColumn: "Quote_Interest_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
