using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth_api.Models;
using auth_api.Services;
using auth_api.Services.Result;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Tests.Api.Services {
    public class BlogServiceTest {
        private readonly Mock<BlogDbContext> _blogDbContextMock = new ();
        private readonly BlogService _sut; 
        public BlogServiceTest() {
            _sut = new BlogService(_blogDbContextMock.Object);
        }

        private Mock<DbSet<BlogPostDto>> SetupDbSetMock() {
            var posts = new List<BlogPostDto> {
                new() {Id = 1, Title = "Test Title", Body = "Test Content", PublishDate = DateTime.Now},
                new() {Id = 2, Title = "Test Title 2", Body = "Test Content 2", PublishDate = DateTime.Now},
                new() {Id = 3, Title = "Test Title 3", Body = "Test Content 3", PublishDate = DateTime.Now},
                new() {Id = 4, Title = "Test Title 4", Body = "Test Content 4", PublishDate = DateTime.Now},
                new() {Id = 5, Title = "Test Title 5", Body = "Test Content 5", PublishDate = DateTime.Now},
                new() {Id = 6, Title = "Test Title 6", Body = "Test Content 6", PublishDate = DateTime.Now},
                new() {Id = 7, Title = "Test Title 7", Body = "Test Content 7", PublishDate = DateTime.Now},
            };
            var dbset = new Mock<DbSet<BlogPostDto>>(MockBehavior.Default);
            dbset.As<IQueryable<BlogPostDto>>()
                .Setup(m => m.Provider)
                .Returns(posts.AsQueryable().Provider);
            dbset.As<IQueryable<BlogPostDto>>()
                .Setup(m => m.Expression)
                .Returns(posts.AsQueryable().Expression);
            dbset.As<IQueryable<BlogPostDto>>()
                .Setup(m => m.ElementType)
                .Returns(posts.AsQueryable().ElementType);
            dbset.As<IQueryable<BlogPostDto>>()
                .Setup(m => m.GetEnumerator())
                .Returns(posts.AsQueryable().GetEnumerator());

            return dbset;
        }

        [Fact]
        public async Task GetAll_ShouldReturnPagedListOfPosts() {
            // Arrange
            var dbSetMock = SetupDbSetMock();
            _blogDbContextMock.Setup(x => x.BlogPosts).Returns(dbSetMock.Object);
            var pagingParameters = new PagingParameters {
                PageNumber = 2,
                PageSize = 2
            };
            
            // Act
            var result = (PagedServiceResult<BlogPostDto>)await _sut.GetAll(pagingParameters);
            
            // Assert
            Assert.Equal(2, result.Data.Count());
            Assert.Equal(3, result.Data.First().Id);
            Assert.Equal(4, result.Data.Last().Id);
        }
    }
}