using DataAccess;
using Frontend.Store.State;

namespace Frontend.Store.Editor {
    public class EditorFeature : Fluxor.Feature<EditorState>{
        public override string GetName() => "Editor";
        protected override EditorState GetInitialState() => 
            new(new BlogPostDto(), true, true, false, true);
    }
}