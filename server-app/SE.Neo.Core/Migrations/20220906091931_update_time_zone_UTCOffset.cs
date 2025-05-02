using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_time_zone_UTCOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UTC_Offset",
                table: "Time_Zone",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(
                "update [dbo].[Time_Zone] set [UTC_Offset] = -9.5 where [Time_Zone_Id] = 5 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = -3.5 where[Time_Zone_Id] = 33 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 3.5 where[Time_Zone_Id] = 79 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 4.5 where[Time_Zone_Id] = 88 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5.5 where[Time_Zone_Id] = 93 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5.5 where[Time_Zone_Id] = 94 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5.75 where[Time_Zone_Id] = 95 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 6.5 where[Time_Zone_Id] = 99 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 8.75 where[Time_Zone_Id] = 112 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 9.5 where[Time_Zone_Id] = 118 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 9.5 where[Time_Zone_Id] = 119 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 10.5 where[Time_Zone_Id] = 125 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 12.75 where[Time_Zone_Id] = 137 " 
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "update [dbo].[Time_Zone] set [UTC_Offset] = -9 where [Time_Zone_Id] = 5 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = -3 where[Time_Zone_Id] = 33 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 3 where[Time_Zone_Id] = 79 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 4 where[Time_Zone_Id] = 88 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5 where[Time_Zone_Id] = 93 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5 where[Time_Zone_Id] = 94 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 5 where[Time_Zone_Id] = 95 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 6 where[Time_Zone_Id] = 99 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 8 where[Time_Zone_Id] = 112 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 9 where[Time_Zone_Id] = 118 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 9 where[Time_Zone_Id] = 119 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 10 where[Time_Zone_Id] = 125 " +
                "update [dbo].[Time_Zone] set[UTC_Offset] = 12 where[Time_Zone_Id] = 137 "
                );

            migrationBuilder.AlterColumn<int>(
                name: "UTC_Offset",
                table: "Time_Zone",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
