using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace auth_api.Models {
    /// <summary>
    /// This interface is only used to facilitate dependency injection
    /// to allow for mocking of the database context.
    /// </summary>
    public interface IBlogDbContext {
        DbSet<BlogPostDto> BlogPosts { get; set; }
    }
}