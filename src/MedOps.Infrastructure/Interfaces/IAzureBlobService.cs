namespace MedOps.Infrastructure.Interfaces;

public interface IAzureBlobService
{
    Task UploadBlobAsync(string containerName, string blobName, Stream data, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<bool> BlobExistsAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
}