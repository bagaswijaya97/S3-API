using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly S3Service _s3Service;

    public S3Controller(IConfiguration configuration)
    {
        var credentials = configuration.GetSection("S3Credentials").Get<S3Credentials>();
        var urlPathKey = configuration.GetValue<string>("UrlPathKey");
        _s3Service = new S3Service(credentials, urlPathKey);
    }

    [HttpGet("buckets")]
    public async Task<IActionResult> ListBuckets()
    {
        var buckets = await _s3Service.ListBucketsAsync();
        return Ok(buckets);
    }

    [HttpGet("objects/{bucketName}")]
    public async Task<IActionResult> ListObjects(string bucketName)
    {
        var objects = await _s3Service.ListObjectsAsync(bucketName);
        return Ok(objects);
    }

    [HttpGet("objects/{bucketName}/search")]
    public async Task<IActionResult> SearchObjects(string bucketName, [FromQuery] string pathFileName)
    {
        var objects = await _s3Service.SearchObjectsAsync(bucketName, pathFileName);
        return Ok(objects.Select(obj => new
        {
            FileName = obj.FileName,
            Size = obj.Size,
            Extension = obj.Extension,
            CreatedAt = obj.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"), // Format tanggal dan waktu saat ini
            LastModified = obj.LastModified.ToString("yyyy/MM/dd HH:mm:ss") // Format tanggal dan waktu saat ini
        }));
    }

    [HttpGet("download/{bucketName}/{pathFileName}")]
    public async Task<IActionResult> DownloadObject(string bucketName, string pathFileName)
    {
        var stream = await _s3Service.DownloadObjectAsync(bucketName, pathFileName);
        return File(stream, "application/octet-stream", pathFileName);
    }
}