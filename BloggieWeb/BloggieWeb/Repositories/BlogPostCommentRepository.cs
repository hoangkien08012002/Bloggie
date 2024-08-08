using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb.Repositories
{
    public class BlogPostCommentRepository : IBlogPostComment
    {
        private readonly BloggieDbContext _dbContext;

        public BlogPostCommentRepository(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment comment)
        {
            await _dbContext.BlogPostComments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogByIdAsync(Guid blogPostId)
        {
          return  await _dbContext.BlogPostComments.Where(x => x.Blogpostid == blogPostId).ToListAsync();
        }
    }
}
