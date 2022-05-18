using Fluxor;
using Frontend.Store.State;

namespace Frontend.Store.ImagePicker {
    public class ImagePickerFeature : Feature<ImagePickerState>{
        public override string GetName() => "ImagePickerState";
        protected override ImagePickerState GetInitialState() => new(null, 1, 1);
    }
}