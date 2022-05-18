using System.Threading.Tasks;
using auth_api.Services.Result;
using DataAccess;

namespace auth_api.Services {
    public interface IBlogService {
        Task<IServiceResult> GetAll(PagingParameters pagingParameters);
        Task<IServiceResult> Get(int id);
        Task<IServiceResult> Create(string title, string? preview, string content);
        Task<IServiceResult> Update(int id, BlogPostDto blog);
        Task<IServiceResult> Delete(int id);
    }
}