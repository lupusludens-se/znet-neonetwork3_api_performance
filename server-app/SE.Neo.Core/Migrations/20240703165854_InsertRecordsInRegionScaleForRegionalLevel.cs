using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertRecordsInRegionScaleForRegionalLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into [dbo].[Region_Scale_Initiative] ([CMS_Region_Id], [Scale_Id], [Created_Ts], [Last_Change_Ts])" +
                                "Values(" +
                                "(Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'Africa'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name='Regional (Choose a continent)'),GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'Asia'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'Europe'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'Mexico & Central America'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'Oceania'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'South America'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())," +
                                "((Select CMS_Region_Id from [dbo].[CMS_Region] where Region_Name = 'USA & Canada'), (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)'), GETDATE(), GETDATE())"
               );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [dbo].[Region_Scale_Initiative] where Scale_Id = (Select Initiative_Scale_Id from [dbo].[Initiative_Scale] where Name = 'Regional (Choose a continent)')");
        }
    }
}
