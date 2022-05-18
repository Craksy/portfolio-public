using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Fluxor;
using Frontend.Services;
using MudBlazor;
using Refit;

namespace Frontend.Store.ImagePicker
{
    public class ImagePickerEffects {
        private readonly IStorageData _storageData;
        private readonly ISnackbar _snackbar;

        public ImagePickerEffects(IStorageData storageData, ISnackbar snackbar) {
            this._storageData = storageData;
            _snackbar = snackbar;
        }

        [EffectMethod]
        public async Task FetchBlobs(FetchBlobsPage action, IDispatcher dispatcher){
            ApiResponse<IEnumerable<BlobDto>> result = await _storageData.GetAll(action.Container, action.PageSize);
            if(result.IsSuccessStatusCode){
                dispatcher.Dispatch(new FetchBLobsSuccess(result));
            }
        }
        
        [EffectMethod]
        public async Task UploadBlobEffect(UploadBlob action, IDispatcher dispatcher){
            // the try/catch is to catch the exception thrown by the refit library
            var file = action.File;
            try {
                await _storageData.UploadNew(action.Container,
                    new StreamPart(file.OpenReadStream(), file.Name, file.ContentType),
                    action.FileName);
                _snackbar.Add("File uploaded successfully", Severity.Success);
                dispatcher.Dispatch(new UploadBlobSuccess());
            }
            catch(Exception e) {
                Console.WriteLine("Error: {0}", e.Message);
                _snackbar.Add($"Failed to upload image: {e.Message}", Severity.Error);
                dispatcher.Dispatch(new UploadBlobFail($"Failed to upload image: {e.Message}"));
            }
        }
    }
}