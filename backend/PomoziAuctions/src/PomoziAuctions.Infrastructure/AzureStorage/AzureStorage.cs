using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
using Azure.Storage.Sas;

namespace PomoziAuctions.Infrastructure.AzureStorage;
public class AzureStorage : IBlobStorage
{
	#region Dependency Injection / Constructor

	private readonly string _storageConnectionString;
	private readonly string _storageContainerName;
	private readonly ILogger<AzureStorage> _logger;

	public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
	{
		_storageConnectionString = configuration["BlobConnectionString"];
		_storageContainerName = configuration["BlobContainerName"];
		_logger = logger;
	}

	#endregion

	public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
	{
		BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

		BlobClient file = client.GetBlobClient(blobFilename);

		try
		{
			// Delete the file
			await file.DeleteAsync();
		}
		catch (RequestFailedException ex)
			when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
		{
			// File did not exist, log to console and return new response to requesting method
			_logger.LogError($"File {blobFilename} was not found.");
			return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
		}

		// Return a new BlobResponseDto to the requesting method
		return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

	}

	public async Task<CreateBlobDto> DownloadAsync(string blobFilename)
	{
		// Get a reference to a container named in appsettings.json
		BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

		try
		{
			// Get a reference to the blob uploaded earlier from the API in the container from configuration settings
			BlobClient file = client.GetBlobClient(blobFilename);

			// Check if the file exists in the container
			if (await file.ExistsAsync())
			{
				var data = await file.OpenReadAsync();
				Stream blobContent = data;

				// Download the file details async
				var content = await file.DownloadContentAsync();

				// Add data to variables in order to return a BlobDto
				string contentType = content.Value.Details.ContentType;

				// Create new BlobDto with blob data from variables
				return new CreateBlobDto { Content = blobContent, FileName = blobFilename, ContentType = contentType };
			}
		}
		catch (RequestFailedException ex)
			when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
		{
			// Log error to console
			_logger.LogError($"File {blobFilename} was not found.");
		}

		// File does not exist, return null and handle that in requesting method
		return null;
	}

	public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
	{
		// Create new upload response object that we can return to the requesting method
		BlobResponseDto response = new();
		response.Blob.FileName = blob.FileName;
		response.Blob.NormalizedName = Guid.NewGuid().ToString();

		// Get a reference to a container named in appsettings.json and then create it
		BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
		//await container.CreateAsync();
		try
		{
			// Get a reference to the blob just uploaded from the API in a container from configuration settings
			BlobClient client = container.GetBlobClient(response.Blob.NormalizedName);

			// Open a stream for the file we want to upload
			await using (Stream? data = blob.OpenReadStream())
			{
				// Upload the file async
				await client.UploadAsync(data);
			}

			// Everything is OK and file got uploaded
			response.Status = $"File {blob.FileName} Uploaded Successfully";
			response.Error = false;
			response.Blob.Uri = client.Uri.AbsoluteUri;

		}
		// If the file already exists, we catch the exception and do not upload it
		catch (RequestFailedException ex)
			when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
		{
			_logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
			response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
			response.Error = true;
			return response;
		}
		// If we get an unexpected error, we catch it here and return the error message
		catch (RequestFailedException ex)
		{
			// Log error to console and create a new response we can return to the requesting method
			_logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
			response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
			response.Error = true;
			return response;
		}

		// Return the BlobUploadResponse object
		return response;
	}

	/// <summary>
	/// Get the full public URL for a blob by its normalized name.
	/// </summary>
	/// <param name="normalizedName">The unique identifier or normalized name of the blob.</param>
	/// <returns>The full URL of the blob.</returns>
	public string GetBlobUrl(string normalizedName)
	{
		var blobClient = new BlobClient(_storageConnectionString, _storageContainerName, normalizedName);
		return blobClient.Uri.AbsoluteUri;  // Returns the full blob URL
	}

	/// <summary>
	/// Get the full public URL for a blob by its normalized name with a shared access signature (SAS) token.
	/// </summary>
	/// param name="normalizedName">The unique identifier or normalized name of the blob.</param>
	/// <returns>The full URL of the blob with a SAS token.</returns>
	public string GetBlobUrlWithSas(string normalizedName, TimeSpan expiryTime)
	{
		var blobClient = new BlobClient(_storageConnectionString, _storageContainerName, normalizedName);
		var sasBuilder = new BlobSasBuilder
		{
			BlobContainerName = _storageContainerName,
			BlobName = normalizedName,
			ExpiresOn = DateTimeOffset.UtcNow.Add(expiryTime),
			Protocol = SasProtocol.HttpsAndHttp
		};
		sasBuilder.SetPermissions(BlobSasPermissions.Read);

		var sasUri = blobClient.GenerateSasUri(sasBuilder);
		return sasUri.AbsoluteUri;
	}
}
