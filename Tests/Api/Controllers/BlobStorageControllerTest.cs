using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using auth_api.Controllers;
using auth_api.Services;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Api.Controllers {
    public class BlobStorageControllerTests {
        
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IStorageService> _storageServiceMock = new Mock<IStorageService>();
        private readonly BlobStorageController _sut;

        public BlobStorageControllerTests(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            _sut = new BlobStorageController(_storageServiceMock.Object);
        }
        
        /// <summary>
        /// Small helper method to create a mock HttpContext.
        /// Makes it easier to setup a new HttpContext for each test.
        /// </summary>
        private void SetupHttpContext() {
            _sut.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            };
            _sut.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
        }
        
        [Fact]
        public async Task GetFile_ShouldReturnUriIfExists() {
            // Arrange
            const string fileName = "fileName";
            const string containerName = "containerName";
            var uri = new Uri($"http://www.remote.test/{containerName}/{fileName}");
            _storageServiceMock
                .Setup(x => x.GetFile(containerName, fileName))
                .ReturnsAsync(new ServiceSuccess<Uri>(uri));
            
            // Act
            var result = await _sut.GetFile(containerName, fileName);

            // Assert
            Assert.Equal(uri, ((OkObjectResult)result.Result).Value);
        }
        
        [Fact]
        public async Task GetFile_ShouldReturnNotFound() {
            // Arrange
            _storageServiceMock
                .Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceError("File not found"));
            
            // Act
            var result = await _sut.GetFile("invalid", "request");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async Task GetAllFiles_ShouldReturnPage() {
            // Arrange
            SetupHttpContext();
            const string fileName = "fileName";
            const string containerName = "containerName";
            var uri = new Uri($"http://www.remote.test/{containerName}");
            var blobsDtos = new BlobDto[] {
                new(fileName, uri.ToString(), "ContentType")
            };
            
            var pagingParams = new PagingParameters{ PageNumber = 42 , PageSize = 1337 };
            _storageServiceMock
                .Setup(x => x.GetAllFiles(containerName, It.IsAny<PagingParameters>()))
                .ReturnsAsync(new PagedServiceResult<BlobDto>(blobsDtos, pagingParams));
            
            // Act
            var result = await _sut.Index(containerName, new PagingParameters());
            _testOutputHelper.WriteLine(result.ToString());
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task GetAllFiles_ShouldAttachHeaders() {
            // Arrange
            SetupHttpContext();
            var requestPagingParams = new PagingParameters{ PageNumber = 42 , PageSize = 1337 };
            var responsePagingParameters = new PagingParameters{ PageNumber = 42 , PageSize = 1337, TotalCount = 420};
            _storageServiceMock
                .Setup(x => x.GetAllFiles(It.IsAny<string>(), It.IsAny<PagingParameters>()))
                .ReturnsAsync(new PagedServiceResult<BlobDto>(new List<BlobDto>(), responsePagingParameters));
            
            // Act
            await _sut.Index("", requestPagingParams);
            
            // Assert
            var headers = _sut.ControllerContext.HttpContext.Response.Headers;
            PagingParameters resultPagingParams = null;
            if(headers.TryGetValue("x-pagination", out var values)) {
                resultPagingParams = JsonSerializer.Deserialize<PagingParameters>(values[0]);
            }
            Assert.Equal(requestPagingParams.PageNumber, resultPagingParams?.PageNumber);
            Assert.Equal(requestPagingParams.PageSize, resultPagingParams?.PageSize);
            Assert.Equal(responsePagingParameters.TotalCount, resultPagingParams?.TotalCount);
        }
        
        [Fact]
        public async Task DeleteFile_ShouldReturnOkIfExists() {
            // Arrange
            SetupHttpContext();
            const string fileName = "fileName";
            const string containerName = "containerName";
            _storageServiceMock
                .Setup(x => x.DeleteFile(containerName, fileName))
                .ReturnsAsync(new ServiceSuccess());
            
            // Act
            var result = await _sut.Delete(containerName, fileName);
            
            // Assert
            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public async Task DeleteFile_ShouldReturnNotFoundIfDoesNotExist() {
            // Arrange
            SetupHttpContext();
            const string fileName = "fileName";
            const string containerName = "containerName";
            _storageServiceMock
                .Setup(x => x.DeleteFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceError("File not found"));
            
            // Act
            var result = await _sut.Delete(containerName, fileName);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async Task UploadFile_ShouldReturnOkOnSuccess() {
            // Arrange
            SetupHttpContext();
            const string fileName = "fileName.jpg";
            const string containerName = "containerName";
            var file = new FormFile(new MemoryStream(), 0, 0, fileName, fileName);
            var ffc = new FormFileCollection {file};
            _sut.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), ffc);
            _storageServiceMock
                .Setup(x => x.UploadFile(containerName, file))
                .ReturnsAsync(new ServiceSuccess<Uri>(new Uri($"http://remote.test/api/BlobStorage/{containerName}/{fileName}")));
            
            // Act
            var result = await _sut.Upload(containerName);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task UploadFile_ShouldReturnBadRequestOnFailure() {
            // Arrange
            SetupHttpContext();
            const string fileName = "fileName.jpg";
            const string containerName = "containerName";
            var file = new FormFile(new MemoryStream(), 0, 0, fileName, fileName);
            var ffc = new FormFileCollection {file};
            _sut.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), ffc);
            _storageServiceMock
                .Setup(x => x.UploadFile(It.IsAny<string>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ServiceError("File not found"));
            
            // Act
            var result = await _sut.Upload(containerName);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
    }
}