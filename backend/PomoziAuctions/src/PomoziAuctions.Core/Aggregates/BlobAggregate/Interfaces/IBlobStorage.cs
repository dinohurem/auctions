using Microsoft.AspNetCore.Http;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;

public interface IBlobStorage
{
  /// <summary>
  /// This method uploads a file submitted with the request
  /// </summary>
  /// <param name="file">File for upload</param>
  /// <returns>Blob with status</returns>
  Task<BlobResponseDto> UploadAsync(IFormFile file);

  /// <summary>
  /// This method downloads a file with the specified filename
  /// </summary>
  /// <param name="blobFilename">Filename</param>
  /// <returns>Blob</returns>
  Task<CreateBlobDto> DownloadAsync(string blobFilename);

  /// <summary>
  /// This method deleted a file with the specified filename
  /// </summary>
  /// <param name="blobFilename">Filename</param>
  /// <returns>Blob with status</returns>
  Task<BlobResponseDto> DeleteAsync(string blobFilename);

  /// <summary>
  /// Get the full public URL for a blob by its normalized name.
  /// </summary>
  /// <param name="normalizedName">The unique identifier or normalized name of the blob.</param>
  /// <returns>The full URL of the blob.</returns>
  public string GetBlobUrl(string normalizedName);

  /// <summary>
  /// Get the full public URL for a blob by its normalized name with a shared access signature (SAS) token.
  /// </summary>
  /// param name="normalizedName">The unique identifier or normalized name of the blob.</param>
  /// <returns>The full URL of the blob with a SAS token.</returns>
  public string GetBlobUrlWithSas(string normalizedName, TimeSpan expiryTime);
}
