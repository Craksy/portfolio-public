using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataAccess;
using Refit;

namespace Frontend.Models
{

    /// <summary>
    /// Extension method that creates a new PagedResult from an ApiResponse
    /// </summary>
    public static class ResponseExtension{
        public static PagedResult<T> AsPages<T>(this ApiResponse<IEnumerable<T>> response) => new(response);
    }

    /// <summary>
    /// A wrapper around an enumerable ApiResponse that allows for easy paging
    /// </summary>
    /// <typeparam name="T">The type of object to be pages</typeparam>
    public class PagedResult<T> : IEnumerable<T>{
        private readonly IApiResponse<IEnumerable<T>> _response;
        private readonly PagingParameters _pages;

        public int PageNumber   => _pages.PageNumber;
        public int PageSize     => _pages.PageSize;
        public int TotalCount   => _pages.TotalCount;
        public int PageCount    => TotalCount / PageSize;

        public IEnumerator<T> GetEnumerator() => _response.Content.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_response.Content).GetEnumerator();

        public PagedResult(ApiResponse<IEnumerable<T>> response) {
            _response = response;
            if(response.Headers.TryGetValues("x-pagination", out var values)){
                _pages = JsonSerializer.Deserialize<PagingParameters>(values.First()) ?? new();
            }else{
                throw new Exception("Response did not have header \"x-pagination\"");
            }
        }

    }
}