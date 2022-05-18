using DataAccess;
using Frontend.Models;

namespace Frontend.Store.State
{
    public record ImagePickerState(
        PagedResult<BlobDto>? Blobs, 
        int CurrentPage, 
        int PageCount);
}