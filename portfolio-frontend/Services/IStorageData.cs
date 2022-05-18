using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Refit;

namespace Frontend.Services {
    [Headers("Authorization: Bearer")]
    public interface IStorageData {
        [Get("/blobstorage/{container}")]
        Task<ApiResponse<IEnumerable<BlobDto>>> GetAll(string container, int? pageSize=null, int? pageNumber=null);
        
        [Multipart("Foo bar?")]
        [Post("/blobstorage/{container}"), Headers("Authorization: Bearer")]
        Task<IApiResponse> UploadNew(string container, [AliasAs("image")]StreamPart file, [Query] string filename);
        
        [Delete("/blobstorage/{container}/{filename}")]
        Task<IApiResponse> Delete(string container, string filename);
    }
}