namespace SE.Neo.Infrastructure.Models.AzureSearch
{
    public class SearchFilter
    {
        public string Expression { get; set; }

        public string Field { get; set; }

        public string Value { get; set; }

        public string Operator { get; set; }

        public string CreateODataFilter()
        {
            if (!string.IsNullOrEmpty(Expression))
            {
                return Expression;
            }

            return $"{Field} {Operator} {Value}";
        }
    }
}
