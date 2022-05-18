using DataAccess;

namespace Frontend.Store.State
{
    public record EditorState(
        BlogPostDto Model,
        bool AutoRefresh,
        bool ShowPreview,
        bool IsSubmitting,
        bool IsNewPost
    ) : StateBase;
}