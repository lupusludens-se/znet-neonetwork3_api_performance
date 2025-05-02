using CsvHelper.Configuration.Attributes;

namespace SE.Neo.CompanyLogoExporter.Models
{
    public class CompanyExportRequest
    {
        public string Name { get; set; }

        [Name("Company Logo")]
        public string CompanyLogoURL { get; set; }
    }
}
