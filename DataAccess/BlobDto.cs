namespace DataAccess {
    public record BlobDto(string Name, string Uri, string ContentType);
    public record ContainerInfo(string Name, string? PublicAccess);
}