using System.Collections.Generic;
using System.Threading.Tasks;
using auth_api.Services;
using auth_api.Services.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DataAccess;
using PagingParameters = DataAccess.PagingParameters;

namespace auth_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) {
            _blogService = blogService;
        }
        
        /// <summary>
        ///     A small helper method to add headers to the response
        /// </summary>
        private TResult WithHeader<T, TResult>(TResult result, string header, T value) where TResult : IActionResult {
            Response.Headers.Add(header, JsonConvert.SerializeObject(value));
            return result;
        }

        
        /// <summary>
        /// Gets a page 
        /// </summary>
        /// <param name="pageParams"></param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPages([FromQuery] PagingParameters pageParams) 
            => await _blogService.GetAll(pageParams) switch {
                PagedServiceResult<BlogPostDto> res => WithHeader(Ok(res.Data), "x-pagination", res.PageParameters),
                {Message: var msg} => NotFound(msg),
            };

        /// <summary>
        /// Gets a single blog post
        /// </summary>
        /// <param name="id">blog post ID</param>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlogPostDto>> GetById(int id)
            => await _blogService.Get(id) switch {
                ServiceSuccess<BlogPostDto> res => Ok(res.Data),
                {Message: var msg} => NotFound(msg),
            };

        /// <summary>
        /// Creates a new blog post
        /// </summary>
        [Authorize("create:post"), HttpPost]
        public async Task<ActionResult<BlogPostDto>> Create(string title, string? preview, [FromBody] string body) 
            => await _blogService.Create(title, preview, body) switch {
                ServiceSuccess<BlogPostDto> res => CreatedAtAction(nameof(GetById), new {id = res.Data.Id}, res.Data),
                {Message: var msg} => BadRequest(msg),
            };

        /// <summary>
        /// update an existing blog post
        /// </summary>
        /// <param name="id">id of the post, for validation purposes</param>
        /// <param name="blogPost">The updated blog post object</param>
        [Authorize("update:post"), HttpPut("{id:int}")]
        public async Task<ActionResult<BlogPostDto>> Update(int id, [FromBody] BlogPostDto blogPost) 
            => await _blogService.Update(id, blogPost) switch {
                ServiceSuccess<BlogPostDto> res => Ok(res.Data),
                {Message: var msg} => NotFound(msg),
            };

        
        /// <summary>
        /// Delete a blog post
        /// </summary>
        /// <param name="id">id of the post to delete</param>
        [Authorize("delete:post"), HttpDelete("{id:int}")]
        public async Task<ActionResult<BlogPostDto>> Delete(int id) 
            => await _blogService.Delete(id) switch {
                ServiceSuccess => Ok(),
                {Message: var msg} => NotFound(msg),
            };
    }
}