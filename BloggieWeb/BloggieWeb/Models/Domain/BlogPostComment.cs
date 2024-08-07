namespace BloggieWeb.Models.Domain
{
    public class BlogPostComment
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid Blogpostid { get; set; }

        public Guid Userid { get; set; }

        public DateTime DateAdded { get; set; } 
    }
}
