using System;
using System.Linq;
using System.Threading.Tasks;
using auth_api.Models;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace auth_api.Services {
    public class BlogService : IBlogService {
        private readonly BlogDbContext _context;
        public BlogService(BlogDbContext context) {
            _context = context;
        } 
        
        /// <summary>
        /// Gets a page of blog posts
        /// </summary>
        /// <param name="pagingParameters">Paging information</param>
        /// <returns>A <see cref="PagedServiceResult{BlogPostDto}"/> of the response data</returns>
        public async Task<IServiceResult> GetAll(PagingParameters pagingParameters) {
            try {
                var totalCount = _context.BlogPosts.Count();
                var posts =  await _context.BlogPosts
                    .AsQueryable()
                    .Skip(pagingParameters.PageSize * (pagingParameters.PageNumber - 1))
                    .Take(pagingParameters.PageSize)
                    .ToListAsync();
                pagingParameters.TotalCount = totalCount;
                return new PagedServiceResult<BlogPostDto>(posts, pagingParameters);
            } catch (Exception ex) {
                return new ServiceError("Error while getting blog posts", ex.Message);
            }
        }

        /// <summary>
        ///   Gets a single blog post
        /// </summary>
        /// <param name="id">The ID of the blog post to retrieve</param>
        /// <returns>A <see cref="ServiceSuccess{BlogPostDto}"/> with the fetched post</returns>
        public async Task<IServiceResult> Get(int id) {
            var post = await _context.BlogPosts.AsQueryable().FirstOrDefaultAsync(p => p.Id == id);
            return post != null ? new ServiceSuccess<BlogPostDto>(post) : new ServiceError("Blog post not found");
        }

        /// <summary>
        ///   Creates a new blog post in the database
        /// </summary>
        /// <param name="title"></param>
        /// <param name="preview"></param>
        /// <param name="content"></param>
        /// <returns>The created post with the assigned ID for the sake of returning a CreatedAtAction</returns>
        public async Task<IServiceResult> Create(string title, string? preview, string content) {
            try {
                 preview ??= content[..Math.Min(content.IndexOf("\n\n", StringComparison.InvariantCultureIgnoreCase), 500)];
            } catch (ArgumentOutOfRangeException) {
                 preview = "";
            }
            var post = new BlogPostDto {
                Title = title,
                Preview = preview ?? "",
                Body = content,
                PublishDate = DateTime.Now
            };
            try {
                await _context.BlogPosts.AddAsync(post);
                await _context.SaveChangesAsync();
                return new ServiceSuccess<BlogPostDto>(post);
            } catch (Exception ex) {
                return new ServiceError("Error while creating blog post", ex.Message);
            }
        }
        
        /// <summary>
        /// Update s an existing blog post
        /// </summary>
        /// <param name="id">ID of the blogpost - for validation purposes</param>
        /// <param name="post">The actual blog post</param>
        /// <returns>A plain <see cref="ServiceSuccess"/></returns>
        public async Task <IServiceResult> Update(int id, BlogPostDto post) {
            try {
                if(id != post.Id) {
                    return new ServiceError("Blog post id does not match");
                }
                var existingPost = _context.Entry(post);
                if (existingPost == null) {
                    return new ServiceError("Blog post not found");
                }
                existingPost.State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new ServiceSuccess();
            } catch (Exception ex) {
                return new ServiceError("Error while updating blog post", ex.Message);
            }
        }
        
        /// <summary>
        /// Delete a blog post
        /// </summary>
        /// <param name="id">ID of the blog post to delete</param>
        /// <returns>A plain <see cref="ServiceSuccess"/></returns>
        public async Task<IServiceResult> Delete(int id) {
            try {
                var post = await _context.BlogPosts.FindAsync(id);
                if (post == null) {
                    return new ServiceError("Blog post not found");
                }
                _context.BlogPosts.Remove(post);
                await _context.SaveChangesAsync();
                return new ServiceSuccess();
            } catch (Exception ex) {
                return new ServiceError("Error while deleting blog post", ex.Message);
            }
        }
        
    }
}