using System;
using DataAccess;

namespace Frontend.Store.Editor
{
    public enum ChangeMethod { Update, Create }
    
    //Edit new
    public record CreateNewPost;

    //toggle editor settings
    public record ToggleAutorefresh;
    public record TogglePreview;

    //submit editor content
    public record SubmitChanges(BlogPostDto Post, ChangeMethod Method);
    public record SubmitChangeSuccess;
    public record SubmitChangeFail(string ErrorMessage);

    //Update state
    public record TitleChanged(string Title);
    public record ContentChanged(string Content);

    //Load state
    public record LoadPostAction(int PostId, Action Callback);
    public record LoadPostSuccessAction(BlogPostDto Post);
    public record LoadPostFailAction(string ErrorMessage);
}