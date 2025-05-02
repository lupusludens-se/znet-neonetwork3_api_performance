using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatetime_zonedstwindowsname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Daylight_Abbreviation",
                table: "Time_Zone",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Daylight_Name",
                table: "Time_Zone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Windows_Name",
                table: "Time_Zone",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"
                update [dbo].[Time_Zone]
                set [Windows_Name] = [Standard_Name]");

            migrationBuilder.Sql(
                @"
                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'ALDT', [Daylight_Name] = 'Aleutian Daylight Time'
                where [Time_Zone_Id] = 3

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'AKDT', [Daylight_Name] = 'Alaska Daylight Time'
                where [Time_Zone_Id] = 6

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'PDTM', [Daylight_Name] = 'Pacific Daylight Time (Mexico)'
                where [Time_Zone_Id] = 8

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'PDT', [Daylight_Name] = 'Pacific Daylight Time'
                where [Time_Zone_Id] = 10

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'MDTM', [Daylight_Name] = 'Mountain Daylight Time (Mexico)'
                where [Time_Zone_Id] = 12

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'MDT', [Daylight_Name] = 'Mountain Daylight Time'
                where [Time_Zone_Id] = 13

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CDT', [Daylight_Name] = 'Central Daylight Time'
                where [Time_Zone_Id] = 16

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EASST', [Daylight_Name] = 'Easter Island Summer Time'
                where [Time_Zone_Id] = 17

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CDTM', [Daylight_Name] = 'Central Daylight Time (Mexico)'
                where [Time_Zone_Id] = 18

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EDTM', [Daylight_Name] = 'Eastern Daylight Time (Mexico)'
                where [Time_Zone_Id] = 21

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EDT', [Daylight_Name] = 'Eastern Daylight Time'
                where [Time_Zone_Id] = 22

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'HDT', [Daylight_Name] = 'Haiti Daylight Time'
                where [Time_Zone_Id] = 23

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CUDT', [Daylight_Name] = 'Cuba Daylight Time'
                where [Time_Zone_Id] = 24

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'USEDT', [Daylight_Name] = 'Eastern Daylight Time'
                where [Time_Zone_Id] = 25

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'TCDT', [Windows_Name] = 'Turks And Caicos Standard Time', [Daylight_Name] = 'Turks and Caicos Daylight Time'
                where [Time_Zone_Id] = 26

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'PST', [Daylight_Name] = 'Paraguay Summer Time'
                where [Time_Zone_Id] = 27

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'ADT', [Daylight_Name] = 'Atlantic Daylight Time'
                where [Time_Zone_Id] = 28

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CST', [Daylight_Name] = 'Chile Summer Time'
                where [Time_Zone_Id] = 32

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'NLDT', [Daylight_Name] = 'Newfoundland Standard Time'
                where [Time_Zone_Id] = 33

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'WGST', [Daylight_Name] = 'West Greenland Summer Time'
                where [Time_Zone_Id] = 38

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'PMDT', [Daylight_Name] = 'Saint Pierre Daylight Time'
                where [Time_Zone_Id] = 41

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'AZOST', [Daylight_Name] = 'Azores Summer Time', [Abbreviation] = 'AZO'
                where [Time_Zone_Id] = 45

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Cape Verde Standard Time'
                where [Time_Zone_Id] = 45

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'UTC'
                where [Time_Zone_Id] = 47

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'BST', [Daylight_Name] = 'British Summer Time'
                where [Time_Zone_Id] = 48

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'MRCDT', [Daylight_Name] = 'Morocco Daylight Time'
                where [Time_Zone_Id] = 51

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CEST', [Daylight_Name] = 'Central European Summer Time'
                where [Time_Zone_Id] = 52

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CEST', [Daylight_Name] = 'Central European Summer Time'
                where [Time_Zone_Id] = 53

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CEST', [Daylight_Name] = 'Central European Summer Time'
                where [Time_Zone_Id] = 54

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CEST', [Daylight_Name] = 'Central European Summer Time'
                where [Time_Zone_Id] = 55

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CEST', [Daylight_Name] = 'Central European Summer Time'
                where [Time_Zone_Id] = 57

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EEST', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 58

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EEST', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 59

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EEST', [Windows_Name] = 'E. Europe Standard Time', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 61

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EEST', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 62

                update [dbo].[Time_Zone]
                set[Daylight_Abbreviation] = 'EEST', [Windows_Name] = 'West Bank Standard Time', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 63

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'EEST', [Daylight_Name] = 'Eastern European Summer Time'
                where [Time_Zone_Id] = 65

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'JDT', [Windows_Name] = 'Israel Standard Time', [Daylight_Name] = 'Jerusalem Daylight Time'
                where [Time_Zone_Id] = 66

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Kaliningrad Standard Time'
                where [Time_Zone_Id] = 68

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Russian Standard Time'
                where [Time_Zone_Id] = 76

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Russia Time Zone 3'
                where [Time_Zone_Id] = 83

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Ekaterinburg Standard Time'
                where [Time_Zone_Id] = 90

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'North Asia Standard Time'
                where [Time_Zone_Id] = 103

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'N. Central Asia Standard Time'
                where [Time_Zone_Id] = 104

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'North Asia East Standard Time'
                where [Time_Zone_Id] = 107

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Singapore Standard Time'
                where [Time_Zone_Id] = 108

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Yakutsk Standard Time'
                where [Time_Zone_Id] = 117

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'ACDT', [Daylight_Name] = 'Australian Central Daylight Time'
                where [Time_Zone_Id] = 118

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'AEDT', [Daylight_Name] = 'Australian Eastern Daylight Time'
                where [Time_Zone_Id] = 121

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'TSMDT', [Daylight_Name] = 'Australian Eastern Daylight Time'
                where [Time_Zone_Id] = 123

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Vladivostok Standard Time'
                where [Time_Zone_Id] = 124

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'LHDT', [Daylight_Name] = 'Lord Howe Daylight Time'
                where [Time_Zone_Id] = 125

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Russia Time Zone 10'
                where [Time_Zone_Id] = 127

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'NFDT', [Daylight_Name] = 'Norfolk Island Daylight Time'
                where [Time_Zone_Id] = 129

                update [dbo].[Time_Zone]
                set [Windows_Name] = 'Russia Time Zone 11'
                where [Time_Zone_Id] = 132

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'NZDT', [Daylight_Name] = 'New Zealand Daylight Time'
                where [Time_Zone_Id] = 133

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'FJST', [Daylight_Name] = 'Fiji Summer Time'
                where [Time_Zone_Id] = 135

                update [dbo].[Time_Zone]
                set [Daylight_Abbreviation] = 'CHADT', [Daylight_Name] = 'Chatham Daylight Time'
                where [Time_Zone_Id] = 137");

            //obsolete time zones
            migrationBuilder.Sql("delete from [dbo].[Time_Zone] where [Time_Zone_Id] = 136");
            migrationBuilder.Sql("delete from [dbo].[Time_Zone] where [Time_Zone_Id] = 44");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Daylight_Abbreviation",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Daylight_Name",
                table: "Time_Zone");

            migrationBuilder.DropColumn(
                name: "Windows_Name",
                table: "Time_Zone");
        }
    }
}
