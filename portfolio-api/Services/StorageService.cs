using System;
using System.Linq;
using System.Threading.Tasks;
using auth_api.Services.Result;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;

namespace auth_api.Services {
    public class StorageService : BlobServiceClient, IStorageService {
        public StorageService(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Helper method to get a blob container, or create it if it doesn't exist.
        /// </summary>
        /// <param name="containerName">Name of the container to get</param>
        /// <returns>The BlobContainerClient for the requested container</returns>
        private async Task<BlobContainerClient> GetOrCreateContainer(string containerName) {
            var container = GetBlobContainerClient(containerName);
            if (!await container.ExistsAsync())
                await container.CreateAsync();
            return container;
        }

        /// <summary>
        /// Retrieves a paged list of containers in the storage account.
        /// </summary>
        /// <param name="pageParams">Paging parameters</param>
        /// <returns>Paginated list of containers</returns>
        public async Task<IServiceResult> GetContainersInfo(PagingParameters pageParams) {
            // TODO: Error handling
            var containerPages = GetBlobContainersAsync();
            var totalCount = await containerPages.CountAsync();
            var containers = await containerPages
                .Skip(pageParams.PageSize * (pageParams.PageNumber - 1))
                .Take(pageParams.PageSize)
                .Select(cont => new ContainerInfo(cont.Name, cont.Properties.PublicAccess.ToString()))
                .ToListAsync();
            pageParams.TotalCount = totalCount;
            return new PagedServiceResult<ContainerInfo>(containers, pageParams);
        }

        /// <summary>
        /// Upload a file to a specified container.
        /// </summary>
        /// <param name="containerName">The container to upload a file to</param>
        /// <param name="image">The image file to upload</param>
        /// <returns>An <see cref="IServiceResult"/> indicating the success status of the operation</returns>
        public async Task<IServiceResult> UploadFile(string containerName, IFormFile image) {
            try {
                var container = await GetOrCreateContainer(containerName);
                var blob = container.GetBlobClient(image.FileName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                await using (var fp = image.OpenReadStream()) {
                    await blob.UploadAsync(fp, new BlobHttpHeaders {ContentType = image.ContentType});
                }
                return new ServiceSuccess<Uri>(blob.Uri);
            }
            catch (Exception e) {
                return new ServiceError($"Upload failed: {e}", e.Message);
            }
        }

        /// <summary>
        /// Delete a file from a specified container.
        /// </summary>
        /// <param name="containerName">The container to delete a file from</param>
        /// <param name="blobName">The name of the blob to delete</param>
        /// <returns>An <see cref="IServiceResult"/> indicating the success status of the operation</returns>
        public async Task<IServiceResult> DeleteFile(string containerName, string blobName) {
            try {
                var container = await GetOrCreateContainer(containerName);
                var blob = container.GetBlobClient(blobName);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (Exception e) {
                return new ServiceError( $"Couldn't delete '{blobName}' from '{containerName}': {e.Message}");
            }

            return new ServiceSuccess();
        }

        /// <summary>
        /// Gets a paginated list of blobs in a container.
        /// </summary>
        /// <param name="containerName">The name of the container to list files from</param>
        /// <param name="pageParams">The <see cref="PagingParameters"/> for the request</param>
        /// <returns>A <see cref="PagedServiceResult{T}"/> for the requested data</returns>
        public async Task<IServiceResult> GetAllFiles(string containerName,
            PagingParameters pageParams) {
            try {
                var container = await GetOrCreateContainer(containerName);
                var blobPages = container.GetBlobsAsync();
                var blobs = await blobPages
                    .Skip(pageParams.PageSize * (pageParams.PageNumber - 1))
                    .Take(pageParams.PageSize)
                    .Select(blob => new BlobDto(blob.Name, container.Uri.AbsoluteUri + "/" + blob.Name,
                        blob.Properties.ContentType))
                    .ToListAsync();
                pageParams.TotalCount = await blobPages.CountAsync();
                return new PagedServiceResult<BlobDto>(blobs, pageParams);
            }
            catch (Exception e) {
                return new ServiceError("Failed to get files", e.Message);
            }
        }

        /// <summary>
        /// Get the location of a file in a container.
        /// </summary>
        /// <param name="containerName">The container to get a file from</param>
        /// <param name="filename">The name of the file to get</param>
        /// <returns>A <see cref="ServiceSuccess{Uri}"/> with the address of the requested blob</returns>
        public async Task<IServiceResult> GetFile(string containerName, string filename) {
            var container = await GetOrCreateContainer(containerName);
            var file = container.GetBlobClient(filename).Uri;
            if (file is null)
                return new ServiceError("File not found");
            return new ServiceSuccess<Uri>(file);
        }
    }
}