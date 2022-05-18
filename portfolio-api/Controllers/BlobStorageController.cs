using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using auth_api.Services;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace auth_api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BlobStorageController : ControllerBase {
        private readonly IStorageService _storageService;

        public BlobStorageController(IStorageService storageService) {
            _storageService = storageService;
        }

        /// <summary>
        ///     A small helper method to add headers to the response
        /// </summary>
        private TResult WithHeader<T, TResult>(TResult result, string header, T value) where TResult : IActionResult {
            Response.Headers.Add(header, JsonConvert.SerializeObject(value));
            return result;
        }

        // ??: Is this endpoint really needed? And if so, should it be public?
        // ??: Does pagination even make sense here?
        /// <summary>
        ///     Gets a page of containers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerInfo>>> GetContainers(
            [FromQuery] PagingParameters pageParams) =>
            await _storageService.GetContainersInfo(pageParams) switch {
                PagedServiceResult<ContainerInfo> res => WithHeader(Ok(res.Data), "x-pagination", res.PageParameters),
                {Message: var msg} => BadRequest(msg)
            };

        /// <summary>
        ///     Get the location of the specified file
        /// </summary>
        [HttpGet("{containerName}/{filename}")]
        public async Task<ActionResult<Uri>> GetFile(string containerName, string filename) =>
            await _storageService.GetFile(containerName, filename) switch {
                ServiceSuccess<Uri> {Data: var uri} => Ok(uri),
                _ => NotFound()
            };

        /// <summary>
        ///     Uploads a file to the specified container.
        /// </summary>
        [Authorize("upload:file")]
        [HttpPost("{containerName}")]
        public async Task<IActionResult> Upload(string containerName) =>
            await _storageService.UploadFile(containerName, Request.Form.Files[0]) switch {
                ServiceSuccess<Uri> {Success: true, Data: var uri} => Ok(uri),
                {Message: var message} => BadRequest(message)
            };

        /// <summary>
        ///     Gets a page of files from the specified container.
        /// </summary>
        [HttpGet("{containerName}")]
        public async Task<ActionResult<IEnumerable<BlobDto>>> Index
            (string containerName, [FromQuery] PagingParameters pageParams) =>
            await _storageService.GetAllFiles(containerName, pageParams) switch {
                PagedServiceResult<BlobDto> res => WithHeader(Ok(res.Data), "x-pagination", res.PageParameters),
                {Message: var msg} => BadRequest(msg),
            };

        /// <summary>
        ///     Deletes a file from the specified container.
        /// </summary>
        [Authorize("delete:file")]
        [HttpDelete("{containerName}/{blobName}")]
        public async Task<IActionResult> Delete(string containerName, string blobName) =>
            await _storageService.DeleteFile(containerName, blobName) switch {
                {Success: true} => Ok(),
                {Message: var msg} => NotFound(msg)
            };
    }
}