using System.Threading.Tasks;
using DataAccess;
using Fluxor;
using MudBlazor;
using Refit;
using Fluxor.Blazor.Web.Middlewares.Routing;
using Frontend.Services;

namespace Frontend.Store.Editor
{
    public class EditorEffects {
        private readonly IBlogData _blogData;
        private readonly ISnackbar _snackbar;

        public EditorEffects(IBlogData blogData, ISnackbar snackbar){
            _blogData = blogData;
            _snackbar = snackbar;
        }
        
        [EffectMethod]
        public async Task HandleLoadPost(LoadPostAction action, IDispatcher dispatcher)
        {
            var post = await _blogData.Get(action.PostId);
            if (post.IsSuccessStatusCode) {
                dispatcher.Dispatch(new LoadPostSuccessAction(post.Content));
                action.Callback.Invoke();
            }else {
                dispatcher.Dispatch(new LoadPostFailAction("Post not found"));
                dispatcher.Dispatch(new GoAction("/"));
            }
        }

        [EffectMethod]
        public async Task SubmitChanges(SubmitChanges action, IDispatcher dispatcher){
            var (post, changeMethod) = action;
            var isUpdate = changeMethod == ChangeMethod.Update;
            try{
                IApiResponse<BlogPostDto> result;
                if(isUpdate){
                    result = await _blogData.UpdatePost(post.Id, post);
                }else{
                    result = await _blogData.CreatePost(post.Title, post.Preview, post.Body);
                }
                if(result.IsSuccessStatusCode){
                    _snackbar.Add("Post successfully " + (isUpdate ? "updated":"created"), Severity.Success);
                    dispatcher.Dispatch(new SubmitChangeSuccess());
                    var postId = isUpdate ? post.Id : result.Content.Id;
                    var navAction = new GoAction($"/viewpost/{postId}");
                    dispatcher.Dispatch(navAction);
                }
            }catch(ApiException apiEx){
                _snackbar.Add($"Update failed: {apiEx.ReasonPhrase}", Severity.Error);
                dispatcher.Dispatch(new SubmitChangeFail(apiEx.ReasonPhrase));
            }
        }
    }
}