namespace SE.Neo.Core.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            // Create DB on Server, for testing purpose
            context.Database.EnsureCreated();
        }
    }
}
