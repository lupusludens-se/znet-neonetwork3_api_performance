using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateDataForDiscoverabilityProjectsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
              name: "Discoverability_Projects_Data",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  ProjectType = table.Column<string>(type: "varchar(200)", nullable: false),
                  Technologies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Geography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  ProjectTypeSlug = table.Column<string>(type: "varchar(200)", nullable: true),
                  Created_User_Id = table.Column<string>(type: "varchar(200)", nullable: true),
                  Updated_User_Id = table.Column<string>(type: "varchar(200)", nullable: true),
                  Created_Ts = table.Column<string>(type: "datetime2", nullable: true),
                  Last_Change_Ts = table.Column<string>(type: "datetime2", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Discoverability_Projects_Data", x => x.Id);
              });

            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Aggregated PPAs','Offshore Wind, Battery Storage','Europe')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Aggregated PPAs','Groundmount Solar','US - Illinois')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Aggregated PPAs','Onshore Wind','Czech Republic, Austria, Bulgaria, Croatia, Germany, Hungary')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Community Solar','Groundmount Solar','US - Arkansas')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Community Solar','Groundmount Solar','US - New York')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Community Solar','Groundmount Solar','US - Illinois')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EAC Purchasing','Onshore Wind','South America')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EAC Purchasing','Groundmount Solar','USA & Canada')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EAC Purchasing','Groundmount Solar','South Africa')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Efficiency Equipment Measures','Building Controls, Building Envelope, HVAC, Lighting','USA & Canada')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Efficiency Equipment Measures','Building Controls, Building Envelope, HVAC, Lighting','Oceania')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Efficiency Equipment Measures','Building Controls, Building Envelope, HVAC, Lighting','USA & Canada')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EV Charging and Fleet Electrification','EV Charging','US - Florida')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EV Charging and Fleet Electrification','EV Charging','USA & Canada')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('EV Charging and Fleet Electrification','EV Charging','US - Texas')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Offsite Power Purchase Agreement','Offshore Wind','Asia')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Offsite Power Purchase Agreement','Groundmount Solar','US - All')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Offsite Power Purchase Agreement','Onshore Wind','Europe')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Onsite Solar','Carport Solar','US - All')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Onsite Solar','Groundmount Solar, Rooftop Solar','United Kingdom')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Onsite Solar','Groundmount Solar,Rooftop Solar','US - Illinois')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Renewable Retail Electricity','Groundmount Solar','US - Texas')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Renewable Retail Electricity','Groundmount Solar, Onshore Wind','US - Texas')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Utility Green Tariff','Groundmount Solar','US - All')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Utility Green Tariff','Groundmount Solar','US - California')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Utility Green Tariff','Onshore Wind','US - Arizona')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Emerging Technologies','Green Hydrogen','Canada')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Emerging Technologies','Green Hydrogen','US - Texas')");
            migrationBuilder.Sql("INSERT INTO [dbo].[Discoverability_Projects_Data] ([ProjectType], [Technologies], [Geography]) VALUES ('Emerging Technologies','Green Hydrogen','US - New York')");

            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='aggregated-ppas' WHERE ProjectType='Aggregated PPAs'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='community-solar' WHERE ProjectType='Community Solar'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='eac-purchasing' WHERE ProjectType='EAC purchasing'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='efficiency-equipment-measures' WHERE ProjectType='Efficiency Equipment measures'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='ev-charging-fleet-electrification' WHERE ProjectType='EV Charging and Fleet Electrification'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='offsite-power-purchase-agreement' WHERE ProjectType='Offsite power purchase agreement'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='onsite-solar' WHERE ProjectType='Onsite Solar'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='renewable-retail-electricity' WHERE ProjectType='Renewable retail electricity'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='utility-green-tariff' WHERE ProjectType='Utility green tariff'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='emerging-technologies' WHERE ProjectType='Emerging Technologies'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='battery-storage' WHERE ProjectType='Battery Storage'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='efficiency-audits-consulting' WHERE ProjectType='Efficiency Audits and Consulting'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='carbon-offset-purchasing' WHERE ProjectType='Carbon offset purchasing'");
            migrationBuilder.Sql("UPDATE [dbo].[Discoverability_Projects_Data] SET ProjectTypeSlug='fuel-cells' WHERE ProjectType='Fuel Cells'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
