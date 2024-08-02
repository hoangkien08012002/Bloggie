using BloggieWeb.Data;
using BloggieWeb.Models.Domain;
using BloggieWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BloggieWeb.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext _context;

        public TagRepository(BloggieDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await _context.Tags.ToListAsync();

        }

        public  Task<Tag> GetAsync(Guid id)
        {
          return  _context.Tags.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _context.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.Displayname = tag.Displayname;
                await _context.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingtag = await _context.Tags.FindAsync(id);
           
            if (existingtag != null)
            {
                _context.Tags.Remove(existingtag);
                await _context.SaveChangesAsync();

                return existingtag;
            }
            return null;
        }
    }
}
