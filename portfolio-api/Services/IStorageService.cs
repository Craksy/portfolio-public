using System.Threading.Tasks;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.AspNetCore.Http;

namespace auth_api.Services {
    public interface IStorageService {
        //Task<IServiceResult> GetOrCreateContainer(string containerName);
        Task<IServiceResult> GetContainersInfo(PagingParameters pageParams);
        Task<IServiceResult> UploadFile(string containerName, IFormFile image);
        Task<IServiceResult> DeleteFile(string containerName, string blobName);
        Task<IServiceResult> GetAllFiles(string containerName, PagingParameters pageParams);
        Task<IServiceResult> GetFile(string containerName, string filename);
    }
}