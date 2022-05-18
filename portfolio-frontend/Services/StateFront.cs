using DataAccess;
using Fluxor;
using Frontend.Store.Editor;
using Frontend.Store.ImagePicker;
using Microsoft.AspNetCore.Components.Forms;

namespace Frontend.Services{
    public class StateFront{
        private readonly IDispatcher _dispatcher;

        public StateFront(IDispatcher dispatcher){
            _dispatcher = dispatcher;
        }

        public void SubmitPost(BlogPostDto post, ChangeMethod method){
            _dispatcher.Dispatch(new SubmitChanges(post, method));
        }

        public void UpdateTitle(string text){
            _dispatcher.Dispatch(new TitleChanged(text));
        }

        public void UpdateContent(string text){
            _dispatcher.Dispatch(new ContentChanged(text));
        }

        public void ChangePage(int newPage){
            _dispatcher.Dispatch(new ChangePage(newPage));
        }

        public void FetchBlobs(int pageSize = 9, string container = "test-container") {
            _dispatcher.Dispatch(new FetchBlobsPage(container, 1, pageSize));
        }
        
        public void UploadNewImage(string container, string fileName, IBrowserFile file){
            _dispatcher.Dispatch(new UploadBlob(container, fileName, file));
        }
    }
}