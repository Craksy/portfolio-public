using System.Collections.Generic;
using System.Threading.Tasks;
using auth_api.Controllers;
using auth_api.Services;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Api.Controllers {
    public class BlogControllerTest {

        private readonly Mock<IBlogService> _blogServiceMock = new();
        private readonly BlogController _sut;
        
        public BlogControllerTest() {
            _sut = new BlogController(_blogServiceMock.Object);
        }
        
        private void SetupHttpContext() {
            _sut.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            };
            _sut.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
        }
        
        [Fact]
        public async Task GetPages_ShouldReturnOkWhenExists() {
            // Arrange
            SetupHttpContext();
            var pagingParams = new PagingParameters{ PageNumber = 1, PageSize = 10 };
            _blogServiceMock
                .Setup(x => x.GetAll(pagingParams))
                .ReturnsAsync(new PagedServiceResult<BlogPostDto>(new List<BlogPostDto>(), pagingParams));
            
            // Act
            var result = await _sut.GetPages(pagingParams);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task GetPages_ShouldReturnNotFoundWhenNotExists() {
            // Arrange
            SetupHttpContext();
            var pagingParams = new PagingParameters{ PageNumber = 1, PageSize = 10 };
            _blogServiceMock
                .Setup(x => x.GetAll(pagingParams))
                .ReturnsAsync(new ServiceError("Not Found"));
            
            // Act
            var result = await _sut.GetPages(pagingParams);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task GetById_ShouldReturnOkWhenExists() {
            // Arrange
            SetupHttpContext();
            var id = 1;
            _blogServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(new ServiceSuccess<BlogPostDto>(new BlogPostDto()));
            
            // Act
            var result = await _sut.GetById(id);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task GetById_ShouldReturnNotFoundWhenNotExists() {
            // Arrange
            SetupHttpContext();
            _blogServiceMock
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync(new ServiceError("Not Found"));
            
            // Act
            var result = await _sut.GetById(42);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWhenSuccess() {
            // Arrange
            SetupHttpContext();
            var blogPost = new BlogPostDto();
            _blogServiceMock
                .Setup(x => x.Create("Foo", null, "some content"))
                .ReturnsAsync(new ServiceSuccess<BlogPostDto>(blogPost));
            
            // Act
            var result = await _sut.Create("Foo", null, "some content");
            
            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
        
        [Fact]
        public async Task Create_ShouldReturnBadRequestWhenFailed() {
            // Arrange
            SetupHttpContext();
            _blogServiceMock
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceError("Failed"));
            
            // Act
            var result = await _sut.Create("Foo", null, "some content");
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task Update_ShouldReturnOkWhenSuccess() {
            // Arrange
            SetupHttpContext();
            var id = 1;
            var blogPost = new BlogPostDto { Id = id, Title = "Foo", Body = "some content"};
            _blogServiceMock
                .Setup(x => x.Update(id, blogPost))
                .ReturnsAsync(new ServiceSuccess<BlogPostDto>(blogPost));
            
            // Act
            var result = await _sut.Update(id, blogPost);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task Update_ShouldReturnNotFoundWhenNotExists() {
            // Arrange
            SetupHttpContext();
            _blogServiceMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<BlogPostDto>()))
                .ReturnsAsync(new ServiceError("Failed"));
            
            // Act
            var result = await _sut.Update(42, new BlogPostDto());
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task Delete_ShouldReturnOkWhenSuccess() {
            // Arrange
            SetupHttpContext();
            var id = 1;
            _blogServiceMock
                .Setup(x => x.Delete(id))
                .ReturnsAsync(new ServiceSuccess());
            
            // Act
            var result = await _sut.Delete(id);
            
            // Assert
            Assert.IsType<OkResult>(result.Result);
        }
        
        [Fact]
        public async Task Delete_ShouldReturnNotFoundWhenNotExists() {
            // Arrange
            SetupHttpContext();
            _blogServiceMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(new ServiceError("Failed"));
            
            // Act
            var result = await _sut.Delete(42);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
        
    }
}