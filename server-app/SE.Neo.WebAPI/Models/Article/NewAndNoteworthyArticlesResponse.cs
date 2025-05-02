namespace SE.Neo.WebAPI.Models.Article
{
    public class NewAndNoteworthyArticlesResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public bool? IsPublicArticle { get; set; }
    }
}
