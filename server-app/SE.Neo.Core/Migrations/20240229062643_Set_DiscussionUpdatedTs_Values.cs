using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Set_DiscussionUpdatedTs_Values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE Discussion SET Discussion_Updated_Ts = (SELECT  CASE WHEN DiscussionLatestDate >= ISNULL(MessagesLatestUpdatedDate, '') AND DiscussionLatestDate >= ISNULL(MessagesLatestLikedDate,'') THEN DiscussionLatestDate   WHEN ISNULL(MessagesLatestUpdatedDate,'') >= DiscussionLatestDate AND ISNULL(MessagesLatestUpdatedDate,'') >= ISNULL(MessagesLatestLikedDate,'') THEN MessagesLatestUpdatedDate WHEN MessagesLatestLikedDate IS NULL THEN DiscussionLatestDate ELSE MessagesLatestLikedDate       END AS LatestDate from ( SELECT D.Discussion_Id, MAX(D.Last_Change_Ts) as DiscussionLatestDate,  MAX(M.Last_Change_Ts) as MessagesLatestUpdatedDate, MAX(L.Created_Ts) AS MessagesLatestLikedDate FROM Discussion D LEFT JOIN Message M ON  M.Discussion_Id= D.Discussion_Id LEFT JOIN  Message_Like L ON  L.Message_Id= M.Message_Id WHERE D.Discussion_Id = Discussion.Discussion_Id GROUP BY D.Discussion_Id ) as latestDates ) WHERE Discussion_Type IN (1 ,2);");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Discussion Set Discussion_Updated_Ts = NULL");
        }
    }
}
