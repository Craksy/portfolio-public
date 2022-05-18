using Fluxor;
using DataAccess;
using Frontend.Store.State;

namespace Frontend.Store.Editor
{
    public static class EditorReducers {

        //Settings
        [ReducerMethod(typeof(ToggleAutorefresh))]
        public static EditorState ReduceEditorToggleAutorefresh(EditorState state) =>
            state with { AutoRefresh = !state.AutoRefresh };

        [ReducerMethod(typeof(TogglePreview))]
        public static EditorState ReduceEditorTogglePreview(EditorState state) =>
            state with {ShowPreview = !state.ShowPreview};

        //Submitting
        [ReducerMethod(typeof(SubmitChanges))]
        public static EditorState ReduceSubmit(EditorState state) =>
            state with { IsSubmitting = true };

        [ReducerMethod(typeof(SubmitChangeSuccess))]
        public static EditorState ReduceSubmitSuccess(EditorState state) =>
            state with { Model = new BlogPostDto(), IsSubmitting = false, IsNewPost = true};

        [ReducerMethod(typeof(SubmitChangeFail))]
        public static EditorState ReduceSubmitFail(EditorState state) =>
            state with {IsSubmitting = false };

        [ReducerMethod]
        public static EditorState ReduceTitleChanged(EditorState state, TitleChanged action) =>
            state with { 
                Model = new BlogPostDto(){
                    Id = state.Model.Id,
                    Title = action.Title,
                    Preview = state.Model.Preview,
                    Body = state.Model.Body,
                    PublishDate = state.Model.PublishDate, 
                }
            };

        [ReducerMethod]
        public static EditorState ReduceContentChanged(EditorState state, ContentChanged action) =>
            state with { 
                Model = new BlogPostDto(){
                    Id = state.Model.Id,
                    Title = state.Model.Title,
                    Preview = state.Model.Preview,
                    Body = action.Content,
                    PublishDate = state.Model.PublishDate 
                }
            };

        [ReducerMethod(typeof(LoadPostAction))]
        public static EditorState ReduceLoadPost(EditorState state) => 
            state with {Model = new BlogPostDto(), IsSubmitting = false, IsLoading = true};
        
        [ReducerMethod()]
        public static EditorState ReduceLoadPostSuccess(EditorState state, LoadPostSuccessAction action) => 
            state with {Model = action.Post, IsSubmitting = false, IsLoading = false, IsNewPost = false};
        
        //ReduceCreateNewPost
        [ReducerMethod(typeof(CreateNewPost))]
        public static EditorState ReduceCreateNewPost(EditorState state) =>
            state with {Model = new BlogPostDto(), IsSubmitting = false, IsNewPost = true};
        
    }
}