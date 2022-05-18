using System;

namespace DataAccess {
    
    // TODO: Make dependents (namely the frontend) capable of using an immutable version of this class.
    // Might require splitting it into multiple DTOs to allow for partially constructed objects
    /// <summary>
    /// The blog post model
    /// </summary>
    public class BlogPostDto {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Preview { get; init; }
        public string Body { get; set; }
        public DateTime PublishDate { get; init; }
    }

    /// <summary>
    /// DTOs for the index page and view page respectively
    /// </summary>
    public record BlogPreviewDto(int Id, string Title, string Preview, DateTime PublishDate);
    public record BlogViewDto(int Id, string Title, string Body, DateTime PublishDate);
}