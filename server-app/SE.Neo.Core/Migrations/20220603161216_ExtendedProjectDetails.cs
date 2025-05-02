using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ExtendedProjectDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Battery_Storage_Details_Project_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Project_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Interest_Project_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project_Value_Provided",
                table: "Project_Value_Provided");

            migrationBuilder.DropColumn(
                name: "Additional_Comments",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Time_And_Urgency_Considerations",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "Project_Value_Provided",
                newName: "Value_Provided");

            migrationBuilder.RenameColumn(
                name: "Minimum_Annual",
                table: "Project_Fuel_Cells_Details",
                newName: "Minimum_Annual_Site_KWh");

            migrationBuilder.RenameColumn(
                name: "Project_Value_Provided_Name",
                table: "Value_Provided",
                newName: "Value_Provided_Name");

            migrationBuilder.RenameColumn(
                name: "Project_Value_Provided_Id",
                table: "Value_Provided",
                newName: "Value_Provided_Id");

            migrationBuilder.AddColumn<string>(
                name: "Additional_Comments",
                table: "Project_Fuel_Cells_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Fuel_Cells_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Additional_Comments",
                table: "Project_Battery_Storage_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Battery_Storage_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Value_Provided",
                table: "Value_Provided",
                column: "Value_Provided_Id");

            migrationBuilder.CreateTable(
                name: "Contract_Price",
                columns: table => new
                {
                    Contract_Price_Id = table.Column<int>(type: "int", nullable: false),
                    Contract_Price_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_Price", x => x.Contract_Price_Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Currency_Id = table.Column<int>(type: "int", nullable: false),
                    Currency_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Currency_Id);
                });

            migrationBuilder.CreateTable(
                name: "EAC",
                columns: table => new
                {
                    EAC_Id = table.Column<int>(type: "int", nullable: false),
                    EAC_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EAC", x => x.EAC_Id);
                });

            migrationBuilder.CreateTable(
                name: "ISO_RTO",
                columns: table => new
                {
                    ISO_RTO_Id = table.Column<int>(type: "int", nullable: false),
                    ISO_RTO_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISO_RTO", x => x.ISO_RTO_Id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Type",
                columns: table => new
                {
                    Product_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Type_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Type", x => x.Product_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Carbon_Offsets_Details",
                columns: table => new
                {
                    Project_Carbon_Offsets_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Purchase_Volume = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Carbon_Offsets_Details", x => x.Project_Carbon_Offsets_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Carbon_Offsets_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Carbon_Offsets_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Community_Solar_Details",
                columns: table => new
                {
                    Project_Community_Solar_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual_MWh = table.Column<int>(type: "int", nullable: true),
                    Total_Annual_MWh = table.Column<int>(type: "int", nullable: true),
                    Utility_Territory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project_Available = table.Column<bool>(type: "bit", nullable: true),
                    Project_Availability_Approximate_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Is_Investment_Grade_Credit_Of_Offtaker_Required = table.Column<bool>(type: "bit", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Community_Solar_Details", x => x.Project_Community_Solar_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Community_Solar_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Community_Solar_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Community_Solar_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_EAC_Details",
                columns: table => new
                {
                    Project_EAC_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Purchase_Volume = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_EAC_Details", x => x.Project_EAC_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_EAC_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_EAC_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Efficiency_Audits_And_Consulting_Details",
                columns: table => new
                {
                    Project_Efficiency_Audits_And_Consulting_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Is_Investment_Grade_Credit_Of_Offtaker_Required = table.Column<bool>(type: "bit", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Efficiency_Audits_And_Consulting_Details", x => x.Project_Efficiency_Audits_And_Consulting_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Efficiency_Equipment_Measures_Details",
                columns: table => new
                {
                    Project_Efficiency_Equipment_Measures_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Is_Investment_Grade_Credit_Of_Offtaker_Required = table.Column<bool>(type: "bit", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Efficiency_Equipment_Measures_Details", x => x.Project_Efficiency_Equipment_Measures_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Equipment_Measures_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Emerging_Technology_Details",
                columns: table => new
                {
                    Project_Emerging_Technology_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual_KWh = table.Column<int>(type: "int", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Emerging_Technology_Details", x => x.Project_Emerging_Technology_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Emerging_Technology_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Emerging_Technology_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Emerging_Technology_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_EV_Charging_Details",
                columns: table => new
                {
                    Project_EV_Charging_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Parking_Space = table.Column<int>(type: "int", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_EV_Charging_Details", x => x.Project_EV_Charging_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_EV_Charging_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_EV_Charging_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_EV_Charging_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Onsite_Solar_Details",
                columns: table => new
                {
                    Project_Onsite_Solar_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual_Site_KWh = table.Column<int>(type: "int", nullable: true),
                    Contract_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Onsite_Solar_Details", x => x.Project_Onsite_Solar_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Onsite_Solar_Details_Contract_Structure_Contract_Structure_Id",
                        column: x => x.Contract_Structure_Id,
                        principalTable: "Contract_Structure",
                        principalColumn: "Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Onsite_Solar_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Onsite_Solar_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Renewable_Retail_Details",
                columns: table => new
                {
                    Project_Renewable_Retail_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Minimum_Annual_Site_KWh = table.Column<int>(type: "int", nullable: true),
                    Minimum_Term_Length = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Renewable_Retail_Details", x => x.Project_Renewable_Retail_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Renewable_Retail_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Renewable_Retail_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchase_Option",
                columns: table => new
                {
                    Purchase_Option_Id = table.Column<int>(type: "int", nullable: false),
                    Purchase_Option_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_Option", x => x.Purchase_Option_Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlement_Calculation_Interval",
                columns: table => new
                {
                    Settlement_Calculation_Interval_Id = table.Column<int>(type: "int", nullable: false),
                    Settlement_Calculation_Interval_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement_Calculation_Interval", x => x.Settlement_Calculation_Interval_Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlement_Hub_Or_Load_Zone",
                columns: table => new
                {
                    Settlement_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Settlement_Type_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement_Hub_Or_Load_Zone", x => x.Settlement_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlement_Price_Interval",
                columns: table => new
                {
                    Settlement_Price_Interval_Id = table.Column<int>(type: "int", nullable: false),
                    Settlement_Price_Interval_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement_Price_Interval", x => x.Settlement_Price_Interval_Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlement_Type",
                columns: table => new
                {
                    Settlement_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Settlement_Type_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlement_Type", x => x.Settlement_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Term_Length",
                columns: table => new
                {
                    Term_Length_Id = table.Column<int>(type: "int", nullable: false),
                    Term_Length_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term_Length", x => x.Term_Length_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Renewable_Retail_Details_Purchase_Option",
                columns: table => new
                {
                    Project_Renewable_Retail_Details_Purchase_Option_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Renewable_Retail_Details_Id = table.Column<int>(type: "int", nullable: false),
                    Purchase_Option_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Renewable_Retail_Details_Purchase_Option", x => x.Project_Renewable_Retail_Details_Purchase_Option_Id);
                    table.ForeignKey(
                        name: "FK_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Project_Renewable_Retail_Details_Id",
                        column: x => x.Project_Renewable_Retail_Details_Id,
                        principalTable: "Project_Renewable_Retail_Details",
                        principalColumn: "Project_Renewable_Retail_Details_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Renewable_Retail_Details_Purchase_Option_Purchase_Option_Purchase_Option_Id",
                        column: x => x.Purchase_Option_Id,
                        principalTable: "Purchase_Option",
                        principalColumn: "Purchase_Option_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Offsite_Power_Purchase_Agreement_Details",
                columns: table => new
                {
                    Project_Offsite_Power_Purchase_Agreement_Details = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Is_Existing = table.Column<bool>(type: "bit", nullable: true),
                    ISORTO_Id = table.Column<int>(type: "int", nullable: true),
                    Custom_ISORTO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product_Type_Id = table.Column<int>(type: "int", nullable: true),
                    Commercial_Operation_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PPA_Term_Years_Length = table.Column<int>(type: "int", nullable: true),
                    Total_Project_Nameplate_MWAC_Capacity = table.Column<int>(type: "int", nullable: true),
                    Total_Project_Expected_Annual_MWh_Production_P50 = table.Column<int>(type: "int", nullable: true),
                    Minimum_Offtake_MWh_Volume_Required = table.Column<int>(type: "int", nullable: true),
                    Notes_For_Potential_Offtakers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Settlement_Type_Id = table.Column<int>(type: "int", nullable: true),
                    Settlement_Hub_Or_LoadZone_Id = table.Column<int>(type: "int", nullable: true),
                    Custom_Settlement_Hub_Or_Load_Zone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    For_All_Price_Entries_Currency_Id = table.Column<int>(type: "int", nullable: true),
                    Contract_Price_Per_MWh = table.Column<int>(type: "int", nullable: true),
                    Floating_Market_Swap_Index_Discount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floating_Market_Swap_Floor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floating_Market_Swap_Cap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pricing_Structure_Id = table.Column<int>(type: "int", nullable: true),
                    Upside_Percentage_To_Developer = table.Column<int>(type: "int", nullable: true),
                    Upside_Percentage_To_Offtaker = table.Column<int>(type: "int", nullable: true),
                    Discount_Amount = table.Column<int>(type: "int", nullable: true),
                    EAC_Id = table.Column<int>(type: "int", nullable: true),
                    Custom_EAC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EAC_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Settlement_Price_Interval_Id = table.Column<int>(type: "int", nullable: true),
                    Settlement_Calculation_Interval_Id = table.Column<int>(type: "int", nullable: true),
                    Additional_Notes_For_SE_Operations_Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project_MW_Currently_Available = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Offsite_Power_Purchase_Agreement_Details", x => x.Project_Offsite_Power_Purchase_Agreement_Details);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Contract_Price_Pricing_Structure_Id",
                        column: x => x.Pricing_Structure_Id,
                        principalTable: "Contract_Price",
                        principalColumn: "Contract_Price_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Currency_For_All_Price_Entries_Currency_Id",
                        column: x => x.For_All_Price_Entries_Currency_Id,
                        principalTable: "Currency",
                        principalColumn: "Currency_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_EAC_EAC_Id",
                        column: x => x.EAC_Id,
                        principalTable: "EAC",
                        principalColumn: "EAC_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_ISO_RTO_ISORTO_Id",
                        column: x => x.ISORTO_Id,
                        principalTable: "ISO_RTO",
                        principalColumn: "ISO_RTO_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Product_Type_Product_Type_Id",
                        column: x => x.Product_Type_Id,
                        principalTable: "Product_Type",
                        principalColumn: "Product_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Calculation_Interval_Settlement_Calculation_Interval_Id",
                        column: x => x.Settlement_Calculation_Interval_Id,
                        principalTable: "Settlement_Calculation_Interval",
                        principalColumn: "Settlement_Calculation_Interval_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Hub_Or_Load_Zone_Settlement_Hub_Or_LoadZone_Id",
                        column: x => x.Settlement_Hub_Or_LoadZone_Id,
                        principalTable: "Settlement_Hub_Or_Load_Zone",
                        principalColumn: "Settlement_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Price_Interval_Settlement_Price_Interval_Id",
                        column: x => x.Settlement_Price_Interval_Id,
                        principalTable: "Settlement_Price_Interval",
                        principalColumn: "Settlement_Price_Interval_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Type_Settlement_Type_Id",
                        column: x => x.Settlement_Type_Id,
                        principalTable: "Settlement_Type",
                        principalColumn: "Settlement_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Carbon_Offsets_Details_Term_Length",
                columns: table => new
                {
                    Project_Carbon_Offsets_Details_Term_Length_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Carbon_Offsets_Details_Id = table.Column<int>(type: "int", nullable: false),
                    Term_Length_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Carbon_Offsets_Details_Term_Length", x => x.Project_Carbon_Offsets_Details_Term_Length_Id);
                    table.ForeignKey(
                        name: "FK_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Project_Carbon_Offsets_Details_Id",
                        column: x => x.Project_Carbon_Offsets_Details_Id,
                        principalTable: "Project_Carbon_Offsets_Details",
                        principalColumn: "Project_Carbon_Offsets_Details_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Carbon_Offsets_Details_Term_Length_Term_Length_Term_Length_Id",
                        column: x => x.Term_Length_Id,
                        principalTable: "Term_Length",
                        principalColumn: "Term_Length_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_EAC_Details_Term_Length",
                columns: table => new
                {
                    Project_EAC_Details_Term_Length_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_EAC_Details_Id = table.Column<int>(type: "int", nullable: false),
                    Term_Length_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_EAC_Details_Term_Length", x => x.Project_EAC_Details_Term_Length_Id);
                    table.ForeignKey(
                        name: "FK_Project_EAC_Details_Term_Length_Project_EAC_Details_Project_EAC_Details_Id",
                        column: x => x.Project_EAC_Details_Id,
                        principalTable: "Project_EAC_Details",
                        principalColumn: "Project_EAC_Details_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_EAC_Details_Term_Length_Term_Length_Term_Length_Id",
                        column: x => x.Term_Length_Id,
                        principalTable: "Term_Length",
                        principalColumn: "Term_Length_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Green_Tariffs_Details",
                columns: table => new
                {
                    Project_Green_Tariffs_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utility_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program_Website = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Minimum_Purchase_Volume = table.Column<int>(type: "int", nullable: true),
                    Term_Length_Id = table.Column<int>(type: "int", nullable: true),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Time_And_Urgency_Considerations = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Green_Tariffs_Details", x => x.Project_Green_Tariffs_Details_Id);
                    table.ForeignKey(
                        name: "FK_Project_Green_Tariffs_Details_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Green_Tariffs_Details_Term_Length_Term_Length_Id",
                        column: x => x.Term_Length_Id,
                        principalTable: "Term_Length",
                        principalColumn: "Term_Length_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Green_Tariffs_Details_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided",
                columns: table => new
                {
                    Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Offsite_Power_Purchase_Agreement_Details_Id = table.Column<int>(type: "int", nullable: false),
                    Value_Provided_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided", x => x.Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Id);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Project_Off~",
                        column: x => x.Project_Offsite_Power_Purchase_Agreement_Details_Id,
                        principalTable: "Project_Offsite_Power_Purchase_Agreement_Details",
                        principalColumn: "Project_Offsite_Power_Purchase_Agreement_Details",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Value_Provided_Value_Provided_Id",
                        column: x => x.Value_Provided_Id,
                        principalTable: "Value_Provided",
                        principalColumn: "Value_Provided_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Contract_Price",
                columns: new[] { "Contract_Price_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Contract_Price_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Plain CFD", null },
                    { 2, null, null, null, "Upside Share", null },
                    { 3, null, null, null, "Market Following", null },
                    { 4, null, null, null, "Fixed Discount to Market", null }
                });

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 1,
                column: "Contract_Structure_Name",
                value: "Cash Purchase");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 2,
                column: "Contract_Structure_Name",
                value: "Power Purchase Agreement");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 4,
                column: "Contract_Structure_Name",
                value: "Lease");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 5,
                column: "Contract_Structure_Name",
                value: "Shared Savings");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 6,
                column: "Contract_Structure_Name",
                value: "Guaranteed Savings");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 7,
                column: "Contract_Structure_Name",
                value: "Discount to Tariff");

            migrationBuilder.InsertData(
                table: "Contract_Structure",
                columns: new[] { "Contract_Structure_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Contract_Structure_Name", "Updated_User_Id" },
                values: new object[] { 8, null, null, null, "As-a-service or Alternative Financing", null });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Currency_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Currency_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "USD", null },
                    { 2, null, null, null, "EUR", null },
                    { 3, null, null, null, "GBP", null },
                    { 4, null, null, null, "AUD", null },
                    { 5, null, null, null, "CAD", null },
                    { 6, null, null, null, "MXN", null }
                });

            migrationBuilder.InsertData(
                table: "EAC",
                columns: new[] { "EAC_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "EAC_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "REC", null },
                    { 2, null, null, null, "Green-E REC", null },
                    { 3, null, null, null, "GO", null },
                    { 4, null, null, null, "REGO", null },
                    { 5, null, null, null, "I-REC", null },
                    { 6, null, null, null, "LGC", null },
                    { 7, null, null, null, "Other", null }
                });

            migrationBuilder.InsertData(
                table: "ISO_RTO",
                columns: new[] { "ISO_RTO_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "ISO_RTO_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "PJM", null },
                    { 2, null, null, null, "ISONE", null },
                    { 3, null, null, null, "MISO", null },
                    { 4, null, null, null, "SERC", null },
                    { 5, null, null, null, "SPP", null },
                    { 6, null, null, null, "ERCOT", null },
                    { 7, null, null, null, "WECC", null },
                    { 8, null, null, null, "CAISO", null },
                    { 9, null, null, null, "AESO", null },
                    { 10, null, null, null, "NYISO", null },
                    { 11, null, null, null, "Other", null }
                });

            migrationBuilder.InsertData(
                table: "Product_Type",
                columns: new[] { "Product_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Product_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Energy Only", null },
                    { 2, null, null, null, "Energy with Project EACs", null },
                    { 3, null, null, null, "Energy with Certified Swap EACs", null },
                    { 4, null, null, null, "Retail Delivered Product", null }
                });

            migrationBuilder.InsertData(
                table: "Purchase_Option",
                columns: new[] { "Purchase_Option_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Purchase_Option_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Behind the Meter", null },
                    { 2, null, null, null, "In front of the Meter", null }
                });

            migrationBuilder.InsertData(
                table: "Settlement_Calculation_Interval",
                columns: new[] { "Settlement_Calculation_Interval_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Calculation_Interval_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "Hourly", null });

            migrationBuilder.InsertData(
                table: "Settlement_Calculation_Interval",
                columns: new[] { "Settlement_Calculation_Interval_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Calculation_Interval_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 2, null, null, null, "Monthly", null },
                    { 3, null, null, null, "Semi-annual", null },
                    { 4, null, null, null, "Annual", null }
                });

            migrationBuilder.InsertData(
                table: "Settlement_Hub_Or_Load_Zone",
                columns: new[] { "Settlement_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "AZ-PV", null },
                    { 2, null, null, null, "CA-NP15", null },
                    { 3, null, null, null, "CA-SP15", null },
                    { 4, null, null, null, "CA-ZP26", null },
                    { 5, null, null, null, "ERCOT-H", null },
                    { 6, null, null, null, "ERCOT-N", null },
                    { 7, null, null, null, "ERCOT-S", null },
                    { 8, null, null, null, "ERCOT-W", null },
                    { 9, null, null, null, "MISO-IA", null },
                    { 10, null, null, null, "MISO-Ark", null },
                    { 11, null, null, null, "MISO-Gul", null },
                    { 12, null, null, null, "MISO-IL", null },
                    { 13, null, null, null, "MISO-IN", null },
                    { 14, null, null, null, "MISO-MI", null },
                    { 15, null, null, null, "MISO-MN", null },
                    { 16, null, null, null, "MISO-MO", null },
                    { 17, null, null, null, "MISO-ND", null },
                    { 18, null, null, null, "NY-A", null },
                    { 19, null, null, null, "NY-G", null },
                    { 20, null, null, null, "NY-J", null },
                    { 21, null, null, null, "SPP-S", null },
                    { 22, null, null, null, "SPP-N", null },
                    { 23, null, null, null, "PJM-AEP GEN HUB", null },
                    { 24, null, null, null, "PJM-AEP-DAYTON HUB", null },
                    { 25, null, null, null, "PJM-ATSI GEN HUB", null },
                    { 26, null, null, null, "PJM-CHICAGO GEN HUB", null },
                    { 27, null, null, null, "PJM-CHICAGO HUB", null },
                    { 28, null, null, null, "PJM-DOMINION HUB", null },
                    { 29, null, null, null, "PJM-EASTERN HUB", null },
                    { 30, null, null, null, "PJM-N ILLINOIS HUB", null },
                    { 31, null, null, null, "PJM-N NEW JERSEY HUB", null },
                    { 32, null, null, null, "PJM-N OHIO HUB", null },
                    { 33, null, null, null, "PJM-N WEST INT HUB", null },
                    { 34, null, null, null, "PJM-N WESTERN HUB", null },
                    { 35, null, null, null, "PSEG Load Zone", null },
                    { 36, null, null, null, "ComEd Load Zone", null },
                    { 37, null, null, null, "Dominion Load Zone", null },
                    { 38, null, null, null, "NEPOOL", null },
                    { 39, null, null, null, "NE-CT", null }
                });

            migrationBuilder.InsertData(
                table: "Settlement_Hub_Or_Load_Zone",
                columns: new[] { "Settlement_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 40, null, null, null, "NE-MA", null },
                    { 41, null, null, null, "NE-ME", null },
                    { 42, null, null, null, "NE-NH", null },
                    { 43, null, null, null, "NE-RI", null },
                    { 44, null, null, null, "Other", null }
                });

            migrationBuilder.InsertData(
                table: "Settlement_Price_Interval",
                columns: new[] { "Settlement_Price_Interval_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Price_Interval_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 0, null, null, null, "Day-Ahead", null },
                    { 1, null, null, null, "Real-Time", null },
                    { 2, null, null, null, "Intraday", null },
                    { 3, null, null, null, "Other", null }
                });

            migrationBuilder.InsertData(
                table: "Settlement_Type",
                columns: new[] { "Settlement_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Hub", null },
                    { 2, null, null, null, "Busbar", null },
                    { 3, null, null, null, "Loadzone", null }
                });

            migrationBuilder.InsertData(
                table: "Term_Length",
                columns: new[] { "Term_Length_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Term_Length_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "12 month", null },
                    { 2, null, null, null, "24 month", null },
                    { 3, null, null, null, "36 month", null },
                    { 4, null, null, null, "> 36 month", null }
                });

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 1,
                column: "Value_Provided_Name",
                value: "Cost Savings");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 2,
                column: "Value_Provided_Name",
                value: "Environmental Attributes and/or Carbon Reduction Targets");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 3,
                column: "Value_Provided_Name",
                value: "Story/Publicity");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 6,
                column: "Value_Provided_Name",
                value: "Renewable Attributes");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 7,
                column: "Value_Provided_Name",
                value: "Energy Arbitrage");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 8,
                column: "Value_Provided_Name",
                value: "Visible comittment to climate action");

            migrationBuilder.InsertData(
                table: "Value_Provided",
                columns: new[] { "Value_Provided_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Value_Provided_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 9, null, null, null, "Community Benefits", null },
                    { 10, null, null, null, "Energy Savings/Reduction", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Value_Provided_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Project_Carbon_Offsets_Details_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length",
                column: "Project_Carbon_Offsets_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Term_Length_Term_Length_Id",
                table: "Project_Carbon_Offsets_Details_Term_Length",
                column: "Term_Length_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Contract_Structure_Id",
                table: "Project_Community_Solar_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Value_Provided_Id",
                table: "Project_Community_Solar_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Value_Provided_Id",
                table: "Project_EAC_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Term_Length_Project_EAC_Details_Id",
                table: "Project_EAC_Details_Term_Length",
                column: "Project_EAC_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Term_Length_Term_Length_Id",
                table: "Project_EAC_Details_Term_Length",
                column: "Term_Length_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Value_Provided_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Contract_Structure_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Value_Provided_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Contract_Structure_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Value_Provided_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Contract_Structure_Id",
                table: "Project_EV_Charging_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Value_Provided_Id",
                table: "Project_EV_Charging_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Term_Length_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Term_Length_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Value_Provided_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_EAC_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "EAC_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_For_All_Price_Entries_Currency_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "For_All_Price_Entries_Currency_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_ISORTO_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "ISORTO_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Pricing_Structure_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Pricing_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Product_Type_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Product_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Calculation_Interval_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Settlement_Calculation_Interval_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Hub_Or_LoadZone_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Settlement_Hub_Or_LoadZone_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Price_Interval_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Settlement_Price_Interval_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Settlement_Type_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Settlement_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Project_Offsite_Power_Purchase_Agreement_Details_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided",
                column: "Project_Offsite_Power_Purchase_Agreement_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Contract_Structure_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Value_Provided_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Value_Provided_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Value_Provided_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Project_Renewable_Retail_Details_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option",
                column: "Project_Renewable_Retail_Details_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Purchase_Option_Purchase_Option_Id",
                table: "Project_Renewable_Retail_Details_Purchase_Option",
                column: "Purchase_Option_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Battery_Storage_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Interest_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest",
                column: "Value_Provided_Id",
                principalTable: "Value_Provided",
                principalColumn: "Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Battery_Storage_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Quote_Interest_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest");

            migrationBuilder.DropTable(
                name: "Project_Carbon_Offsets_Details_Term_Length");

            migrationBuilder.DropTable(
                name: "Project_Community_Solar_Details");

            migrationBuilder.DropTable(
                name: "Project_EAC_Details_Term_Length");

            migrationBuilder.DropTable(
                name: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropTable(
                name: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropTable(
                name: "Project_Emerging_Technology_Details");

            migrationBuilder.DropTable(
                name: "Project_EV_Charging_Details");

            migrationBuilder.DropTable(
                name: "Project_Green_Tariffs_Details");

            migrationBuilder.DropTable(
                name: "Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided");

            migrationBuilder.DropTable(
                name: "Project_Onsite_Solar_Details");

            migrationBuilder.DropTable(
                name: "Project_Renewable_Retail_Details_Purchase_Option");

            migrationBuilder.DropTable(
                name: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropTable(
                name: "Project_EAC_Details");

            migrationBuilder.DropTable(
                name: "Term_Length");

            migrationBuilder.DropTable(
                name: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropTable(
                name: "Project_Renewable_Retail_Details");

            migrationBuilder.DropTable(
                name: "Purchase_Option");

            migrationBuilder.DropTable(
                name: "Contract_Price");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "EAC");

            migrationBuilder.DropTable(
                name: "ISO_RTO");

            migrationBuilder.DropTable(
                name: "Product_Type");

            migrationBuilder.DropTable(
                name: "Settlement_Calculation_Interval");

            migrationBuilder.DropTable(
                name: "Settlement_Hub_Or_Load_Zone");

            migrationBuilder.DropTable(
                name: "Settlement_Price_Interval");

            migrationBuilder.DropTable(
                name: "Settlement_Type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Value_Provided",
                table: "Value_Provided");

            migrationBuilder.DeleteData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "Additional_Comments",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropColumn(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropColumn(
                name: "Additional_Comments",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DropColumn(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.RenameTable(
                name: "Value_Provided",
                newName: "Project_Value_Provided");

            migrationBuilder.RenameColumn(
                name: "Minimum_Annual_Site_KWh",
                table: "Project_Fuel_Cells_Details",
                newName: "Minimum_Annual");

            migrationBuilder.RenameColumn(
                name: "Value_Provided_Name",
                table: "Project_Value_Provided",
                newName: "Project_Value_Provided_Name");

            migrationBuilder.RenameColumn(
                name: "Value_Provided_Id",
                table: "Project_Value_Provided",
                newName: "Project_Value_Provided_Id");

            migrationBuilder.AddColumn<string>(
                name: "Additional_Comments",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time_And_Urgency_Considerations",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project_Value_Provided",
                table: "Project_Value_Provided",
                column: "Project_Value_Provided_Id");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 1,
                column: "Contract_Structure_Name",
                value: "CashPurchase");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 2,
                column: "Contract_Structure_Name",
                value: "PowerPurchaseAgreement");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 4,
                column: "Contract_Structure_Name",
                value: "PPA");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 5,
                column: "Contract_Structure_Name",
                value: "Lease");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 6,
                column: "Contract_Structure_Name",
                value: "SharedSavings");

            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 7,
                column: "Contract_Structure_Name",
                value: "GuaranteedSavings");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 1,
                column: "Project_Value_Provided_Name",
                value: "CostSavings");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 2,
                column: "Project_Value_Provided_Name",
                value: "Environmental");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 3,
                column: "Project_Value_Provided_Name",
                value: "Story");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 6,
                column: "Project_Value_Provided_Name",
                value: "RenewableAttributes");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 7,
                column: "Project_Value_Provided_Name",
                value: "MitigatingClimateChange");

            migrationBuilder.UpdateData(
                table: "Project_Value_Provided",
                keyColumn: "Project_Value_Provided_Id",
                keyValue: 8,
                column: "Project_Value_Provided_Name",
                value: "CommunityBenefits");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Battery_Storage_Details_Project_Value_Provided_Value_Provided_Id",
                table: "Project_Battery_Storage_Details",
                column: "Value_Provided_Id",
                principalTable: "Project_Value_Provided",
                principalColumn: "Project_Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Fuel_Cells_Details_Project_Value_Provided_Value_Provided_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Value_Provided_Id",
                principalTable: "Project_Value_Provided",
                principalColumn: "Project_Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Quote_Interest_Project_Value_Provided_Value_Provided_Id",
                table: "Solar_Quote_Interest",
                column: "Value_Provided_Id",
                principalTable: "Project_Value_Provided",
                principalColumn: "Project_Value_Provided_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
