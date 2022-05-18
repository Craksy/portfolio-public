using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace auth_api.Models
{
    public class BlogDbContext : DbContext, IBlogDbContext
    {
        //public BlogDbContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<BlogPostDto> BlogPosts { get; set; }
    }
}