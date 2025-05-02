using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedSettlementPriceIntervalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 0);

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 1,
                column: "Settlement_Price_Interval_Name",
                value: "Day-Ahead");

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 2,
                column: "Settlement_Price_Interval_Name",
                value: "Real-Time");

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 3,
                column: "Settlement_Price_Interval_Name",
                value: "Intraday");

            migrationBuilder.InsertData(
                table: "Settlement_Price_Interval",
                columns: new[] { "Settlement_Price_Interval_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Price_Interval_Name", "Updated_User_Id" },
                values: new object[] { 4, null, null, null, "Other", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 1,
                column: "Settlement_Price_Interval_Name",
                value: "Real-Time");

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 2,
                column: "Settlement_Price_Interval_Name",
                value: "Intraday");

            migrationBuilder.UpdateData(
                table: "Settlement_Price_Interval",
                keyColumn: "Settlement_Price_Interval_Id",
                keyValue: 3,
                column: "Settlement_Price_Interval_Name",
                value: "Other");

            migrationBuilder.InsertData(
                table: "Settlement_Price_Interval",
                columns: new[] { "Settlement_Price_Interval_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Settlement_Price_Interval_Name", "Updated_User_Id" },
                values: new object[] { 0, null, null, null, "Day-Ahead", null });
        }
    }
}
