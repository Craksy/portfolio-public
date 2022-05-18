namespace auth_api.Services.Result {
    public interface IServiceResult {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}