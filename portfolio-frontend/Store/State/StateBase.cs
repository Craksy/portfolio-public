namespace Frontend.Store.State {
    public record StateBase (bool IsLoading = false, string? ErrorMessage = null){
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    }
}