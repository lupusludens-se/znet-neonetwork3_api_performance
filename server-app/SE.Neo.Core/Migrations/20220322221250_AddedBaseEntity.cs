using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Status",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Status",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Status",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Status",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Profile_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Profile_Technology",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Profile_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Profile_Technology",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Profile_Solution",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Profile_Solution",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Profile_Solution",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Profile_Solution",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Profile_Region",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Profile_Region",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Profile_Region",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Profile_Region",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Profile_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Profile_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Profile_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Profile_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Profile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Profile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "User_Notification",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "User_Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "User_Notification",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "User_Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Tool_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Tool_Saved",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Tool_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Tool_Saved",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Tool_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Tool_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Tool_Role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Tool_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Tool_Company",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Tool_Company",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Tool_Company",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Tool_Company",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Time_Zone",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Time_Zone",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Time_Zone",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Time_Zone",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "State",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "State",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "State",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "State",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Role_Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Role_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Role_Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Role_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Project_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Project_Saved",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Project_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Project_Saved",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Discussion_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Discussion_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Discussion_User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Discussion_User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Country",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Country",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Company_Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Company_Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Company_Location",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Company_Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "CMS_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "CMS_Technology",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "CMS_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "CMS_Technology",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "CMS_Solution",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "CMS_Solution",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "CMS_Solution",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "CMS_Solution",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "CMS_Region",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "CMS_Region",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "CMS_Region",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "CMS_Region",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "CMS_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "CMS_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "CMS_Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "CMS_Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Attachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Attachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Ts",
                table: "Article_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_User_Id",
                table: "Article_Saved",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Last_Change_Ts",
                table: "Article_Saved",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_User_Id",
                table: "Article_Saved",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Status");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Status");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Status");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Status");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Role");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Role");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Role");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Role");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Profile_Technology");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Profile_Technology");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Profile_Technology");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Profile_Technology");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Profile_Solution");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Profile_Solution");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Profile_Solution");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Profile_Solution");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Profile_Region");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Profile_Region");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Profile_Region");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Profile_Region");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Profile_Category");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Profile_Category");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Profile_Category");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Profile_Category");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Permission");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Permission");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Permission");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Permission");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "User_Notification");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "User_Notification");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "User_Notification");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "User_Notification");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Tool_Saved");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Tool_Saved");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Tool_Saved");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Tool_Saved");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Tool_Role");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Tool_Role");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Tool_Role");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Tool_Role");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Tool_Company");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Tool_Company");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Tool_Company");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Tool_Company");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "State");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "State");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "State");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "State");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Role_Permission");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Role_Permission");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Role_Permission");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Role_Permission");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Project_Saved");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Project_Saved");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Project_Saved");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Project_Saved");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Discussion_User");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Discussion_User");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Discussion_User");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Discussion_User");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Company_Location");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Company_Location");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Company_Location");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Company_Location");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "CMS_Category");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "CMS_Category");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "CMS_Category");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "CMS_Category");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "Created_Ts",
                table: "Article_Saved");

            migrationBuilder.DropColumn(
                name: "Created_User_Id",
                table: "Article_Saved");

            migrationBuilder.DropColumn(
                name: "Last_Change_Ts",
                table: "Article_Saved");

            migrationBuilder.DropColumn(
                name: "Updated_User_Id",
                table: "Article_Saved");
        }
    }
}
