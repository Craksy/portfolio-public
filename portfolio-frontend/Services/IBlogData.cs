using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Refit;

namespace Frontend.Services
{
    [Headers("Authorization: Bearer")]
    public interface IBlogData {
        [Get("/blog")]
        Task<ApiResponse<IEnumerable<BlogPreviewDto>>> GetPage(int pageSize=3, int pageNumber=1);
    
        //TODO: Should return api response instead
        [Get("/blog/{id}")]
        Task<IApiResponse<BlogPostDto>> Get(int id);

        [Post("/blog"), Headers("application/json")]
        Task<IApiResponse<BlogPostDto>> CreatePost(string? title, string? preview, [Body(BodySerializationMethod.Serialized)] string body);

        [Put("/blog/{id}"), Headers("Authorization: Bearer")]
        Task<IApiResponse<BlogPostDto>> UpdatePost(int id, [Body] BlogPostDto post);

        [Delete("/blog/{id}"), Headers("Authorization: Bearer")]
        Task DeletePost(int id);
    }
}