using System.Collections.Generic;
using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Refit;

namespace Frontend.Store.ImagePicker
{
    public record ChangePage(int NewPage);

    public record FetchBlobsPage(string Container, int Page, int PageSize);
    public record FetchBLobsSuccess(ApiResponse<IEnumerable<BlobDto>> Pages);
    public record FetchBlobsFail(string Error);
    
    public record UploadBlob(string Container, string FileName, IBrowserFile File);
    public record UploadBlobSuccess;
    public record UploadBlobFail(string Error);
    
}