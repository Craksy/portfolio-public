using System.Collections.Generic;
using DataAccess;

namespace auth_api.Services.Result {
    
    /// <summary>
    /// A result that indicates the success of an operation.
    /// </summary>
    public class ServiceSuccess : IServiceResult {
        public ServiceSuccess(string? message = null) {
            Success = true;
            Message = message;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
    
    /// <summary>
    ///  A result that indicates the success of an operation returning a value.
    /// </summary>
    /// <typeparam name="T">The type of data returned by the service request</typeparam>
    public class ServiceSuccess<T> : ServiceSuccess {
        public ServiceSuccess (T data, string? message = null):base(message) {
            Data = data;
        }
        public T Data { get; }
    }
    
    /// <summary>
    /// A result that indicates the failure of an operation.
    /// </summary>
    public class ServiceError : IServiceResult {
        public ServiceError(string? message, params string[] errors){
            Success = false;
            Message = message;
            //Errors = errors.ToList();
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
        
        //TODO: Is this actually needed?
        //public List<string>? Errors { get; }
    }
    
    /// <summary>
    /// A result that indicates the success of an operation returning paginated data.
    /// </summary>
    /// <typeparam name="T">The type of data being paginated</typeparam>
    public class PagedServiceResult<T> : IServiceResult {
        public PagedServiceResult(IEnumerable<T> data, PagingParameters pageParameters, string? message = null) {
            PageParameters = pageParameters;
            Success = true;
            Message = message;
            Data = data;
        }
        
        public PagingParameters PageParameters { get; }
        public IEnumerable<T> Data { get; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}