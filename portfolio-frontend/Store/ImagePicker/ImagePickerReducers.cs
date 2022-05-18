using Frontend.Models;
using Fluxor;
using Frontend.Store.State;

namespace Frontend.Store.ImagePicker
{
    public class ImagePickerReducers {
        
        [ReducerMethod]
        public static ImagePickerState ReduceFetchSuccess(ImagePickerState state, FetchBLobsSuccess action) {
            return state with {
                Blobs = action.Pages.AsPages()
            };
        }
    }
}