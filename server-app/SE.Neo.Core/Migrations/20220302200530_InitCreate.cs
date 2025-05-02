using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Category",
                columns: table => new
                {
                    CMS_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category_CMS_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Category", x => x.CMS_Category_Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Region",
                columns: table => new
                {
                    CMS_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Region_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parent_Region_Id = table.Column<int>(type: "int", nullable: true),
                    Region_CMS_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Region", x => x.CMS_Region_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Region_CMS_Region_Parent_Region_Id",
                        column: x => x.Parent_Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Solution",
                columns: table => new
                {
                    CMS_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Solution_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solution_CMS_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Solution", x => x.CMS_Solution_Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Technology",
                columns: table => new
                {
                    CMS_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Technology_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Technology_CMS_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Technology", x => x.CMS_Technology_Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Country_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Country_Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Continent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<Point>(type: "geography", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Location_Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Permission_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Permission_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Permission_Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Is_Special = table.Column<bool>(type: "bit", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "Time_Zone",
                columns: table => new
                {
                    Time_Zone_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Display_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Standard_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Has_DST = table.Column<bool>(type: "bit", nullable: false),
                    UTC_Offset = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time_Zone", x => x.Time_Zone_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tool",
                columns: table => new
                {
                    Tool_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Tool_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool", x => x.Tool_Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    State_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    State_Abbr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.State_Id);
                    table.ForeignKey(
                        name: "FK_State_Country_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Country",
                        principalColumn: "Country_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Company_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Technology = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contracts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Term_Length = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payback_Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_EAC_Included = table.Column<bool>(type: "bit", nullable: false),
                    Installed_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Web_Site_Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Default_Location_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Company_Id);
                    table.ForeignKey(
                        name: "FK_Company_Location_Default_Location_Id",
                        column: x => x.Default_Location_Id,
                        principalTable: "Location",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permission",
                columns: table => new
                {
                    Role_Permission_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Permission_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Permission", x => x.Role_Permission_Id);
                    table.ForeignKey(
                        name: "FK_Role_Permission_Permission_Permission_Id",
                        column: x => x.Permission_Id,
                        principalTable: "Permission",
                        principalColumn: "Permission_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Permission_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tool_Role",
                columns: table => new
                {
                    Tool_Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool_Role", x => x.Tool_Role_Id);
                    table.ForeignKey(
                        name: "FK_Tool_Role_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tool_Role_Tool_Tool_Id",
                        column: x => x.Tool_Id,
                        principalTable: "Tool",
                        principalColumn: "Tool_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company_Location",
                columns: table => new
                {
                    Company_Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Location_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Location", x => x.Company_Location_Id);
                    table.ForeignKey(
                        name: "FK_Company_Location_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Location_Location_Location_Id",
                        column: x => x.Location_Id,
                        principalTable: "Location",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Project_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Pinned = table.Column<bool>(type: "bit", nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Project_Id);
                    table.ForeignKey(
                        name: "FK_Project_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tool_Company",
                columns: table => new
                {
                    Tool_Company_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool_Company", x => x.Tool_Company_Id);
                    table.ForeignKey(
                        name: "FK_Tool_Company_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tool_Company_Tool_Tool_Id",
                        column: x => x.Tool_Id,
                        principalTable: "Tool",
                        principalColumn: "Tool_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Is_Onboarded = table.Column<bool>(type: "bit", nullable: false),
                    Image_Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Time_Zone_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_User_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Time_Zone_Time_Zone_Id",
                        column: x => x.Time_Zone_Id,
                        principalTable: "Time_Zone",
                        principalColumn: "Time_Zone_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Permission",
                columns: table => new
                {
                    User_Permission_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Permission_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Permission", x => x.User_Permission_Id);
                    table.ForeignKey(
                        name: "FK_User_Permission_Permission_Permission_Id",
                        column: x => x.Permission_Id,
                        principalTable: "Permission",
                        principalColumn: "Permission_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Permission_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Platform_Notification",
                columns: table => new
                {
                    User_Platform_Notification_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Platform_Notification_Type = table.Column<int>(type: "int", nullable: false),
                    Is_Read = table.Column<bool>(type: "bit", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Platform_Notification", x => x.User_Platform_Notification_Id);
                    table.ForeignKey(
                        name: "FK_User_Platform_Notification_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile",
                columns: table => new
                {
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Job_Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    LinkedIn_Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    About = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Country_Id = table.Column<int>(type: "int", nullable: false),
                    State_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile", x => x.User_Profile_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Country_Country_Id",
                        column: x => x.Country_Id,
                        principalTable: "Country",
                        principalColumn: "Country_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_State_State_Id",
                        column: x => x.State_Id,
                        principalTable: "State",
                        principalColumn: "State_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Role",
                columns: table => new
                {
                    User_Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Role", x => x.User_Role_Id);
                    table.ForeignKey(
                        name: "FK_User_Role_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Conversation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conversation_Type = table.Column<int>(type: "int", nullable: false),
                    User_From_Id = table.Column<int>(type: "int", nullable: false),
                    User_To_Id = table.Column<int>(type: "int", nullable: false),
                    User_Bcc_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Conversation_Id);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_Bcc_Id",
                        column: x => x.User_Bcc_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_From_Id",
                        column: x => x.User_From_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_To_Id",
                        column: x => x.User_To_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Category",
                columns: table => new
                {
                    User_Profile_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Category", x => x.User_Profile_Solution_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Category_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Region",
                columns: table => new
                {
                    User_Profile_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Region_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Region", x => x.User_Profile_Region_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Region_CMS_Region_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Region_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Solution",
                columns: table => new
                {
                    User_Profile_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Solution_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Solution", x => x.User_Profile_Solution_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Solution_CMS_Solution_Solution_Id",
                        column: x => x.Solution_Id,
                        principalTable: "CMS_Solution",
                        principalColumn: "CMS_Solution_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Technology",
                columns: table => new
                {
                    User_Profile_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Technology_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Technology", x => x.User_Profile_Technology_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Technology_CMS_Technology_Technology_Id",
                        column: x => x.Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Message_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Read = table.Column<bool>(type: "bit", nullable: false),
                    Conversation_Id = table.Column<int>(type: "int", nullable: false),
                    User_From_Id = table.Column<int>(type: "int", nullable: false),
                    User_To_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Message_Id);
                    table.ForeignKey(
                        name: "FK_Message_Conversation_Conversation_Id",
                        column: x => x.Conversation_Id,
                        principalTable: "Conversation",
                        principalColumn: "Conversation_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_Profile_User_From_Id",
                        column: x => x.User_From_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_Profile_User_To_Id",
                        column: x => x.User_To_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Attachment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Attachment_Type = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Attachment_Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Message_Message_Id",
                        column: x => x.Message_Id,
                        principalTable: "Message",
                        principalColumn: "Message_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_Message_Id",
                table: "Attachment",
                column: "Message_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Region_Parent_Region_Id",
                table: "CMS_Region",
                column: "Parent_Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Default_Location_Id",
                table: "Company",
                column: "Default_Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Location_Company_Id",
                table: "Company_Location",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Location_Location_Id",
                table: "Company_Location",
                column: "Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_Bcc_Id",
                table: "Conversation",
                column: "User_Bcc_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_From_Id",
                table: "Conversation",
                column: "User_From_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_To_Id",
                table: "Conversation",
                column: "User_To_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Conversation_Id",
                table: "Message",
                column: "Conversation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_User_From_Id",
                table: "Message",
                column: "User_From_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_User_To_Id",
                table: "Message",
                column: "User_To_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Company_Id",
                table: "Project",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permission_Permission_Id",
                table: "Role_Permission",
                column: "Permission_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permission_Role_Id",
                table: "Role_Permission",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_State_Country_Id",
                table: "State",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Company_Company_Id",
                table: "Tool_Company",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Company_Tool_Id",
                table: "Tool_Company",
                column: "Tool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Role_Role_Id",
                table: "Tool_Role",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Role_Tool_Id",
                table: "Tool_Role",
                column: "Tool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Company_Id",
                table: "User",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_First_Name_Last_Name",
                table: "User",
                columns: new[] { "First_Name", "Last_Name" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Time_Zone_Id",
                table: "User",
                column: "Time_Zone_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Permission_Permission_Id",
                table: "User_Permission",
                column: "Permission_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Permission_User_Id",
                table: "User_Permission",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Platform_Notification_User_Id_Is_Read",
                table: "User_Platform_Notification",
                columns: new[] { "User_Id", "Is_Read" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Country_Id",
                table: "User_Profile",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_State_Id",
                table: "User_Profile",
                column: "State_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_User_Id",
                table: "User_Profile",
                column: "User_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Category_Category_Id",
                table: "User_Profile_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Category_User_Profile_Id",
                table: "User_Profile_Category",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Region_Region_Id",
                table: "User_Profile_Region",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Region_User_Profile_Id",
                table: "User_Profile_Region",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Solution_Solution_Id",
                table: "User_Profile_Solution",
                column: "Solution_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Solution_User_Profile_Id",
                table: "User_Profile_Solution",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Technology_Technology_Id",
                table: "User_Profile_Technology",
                column: "Technology_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Technology_User_Profile_Id",
                table: "User_Profile_Technology",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_Role_Id",
                table: "User_Role",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_User_Id",
                table: "User_Role",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Company_Location");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Role_Permission");

            migrationBuilder.DropTable(
                name: "Tool_Company");

            migrationBuilder.DropTable(
                name: "Tool_Role");

            migrationBuilder.DropTable(
                name: "User_Permission");

            migrationBuilder.DropTable(
                name: "User_Platform_Notification");

            migrationBuilder.DropTable(
                name: "User_Profile_Category");

            migrationBuilder.DropTable(
                name: "User_Profile_Region");

            migrationBuilder.DropTable(
                name: "User_Profile_Solution");

            migrationBuilder.DropTable(
                name: "User_Profile_Technology");

            migrationBuilder.DropTable(
                name: "User_Role");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Tool");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "CMS_Category");

            migrationBuilder.DropTable(
                name: "CMS_Region");

            migrationBuilder.DropTable(
                name: "CMS_Solution");

            migrationBuilder.DropTable(
                name: "CMS_Technology");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "User_Profile");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Time_Zone");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
