namespace SE.Neo.Core.Services
{
    public abstract class BaseFilterService
    {
        protected List<int>? ParseFilterByField(string property)
        {
            try
            {
                string idsString = property.Substring(property.IndexOf('=', StringComparison.Ordinal) + 1);
                return idsString.Split(',')
                            .Where(str => int.TryParse(str, out int x))
                            .Select(str => int.Parse(str))
                            .ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
