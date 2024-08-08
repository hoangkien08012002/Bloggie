using BloggieWeb.Models.Domain;

namespace BloggieWeb.Repositories
{
    public interface IBlogPostComment
    {
        Task<BlogPostComment> AddAsync(BlogPostComment comment);

        Task<IEnumerable<BlogPostComment>> GetCommentByBlogByIdAsync( Guid blogPostId);
         
    }
}
