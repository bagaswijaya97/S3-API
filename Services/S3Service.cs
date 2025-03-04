using Amazon.S3;
using Amazon.S3.Model;

public class S3Service
{
    private static AmazonS3Client _s3Client;
    private readonly IAmazonS3 s3Client;
    private readonly string _urlPathKey;

    public S3Service(S3Credentials credentials, string urlPathKey)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = credentials.ServiceUrl,
            ForcePathStyle = true // Sesuaikan jika penyedia S3 memerlukannya
        };

        _s3Client = new AmazonS3Client(credentials.AccessKeyId, credentials.SecretAccessKey, config);
        _urlPathKey = urlPathKey;
    }

    public async Task<List<S3Bucket>> ListBucketsAsync()
    {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets.Select(b => new S3Bucket { Name = b.BucketName, CreationDate = b.CreationDate }).ToList();
    }

    public async Task<List<S3Object>> ListObjectsAsync(string bucketName)
    {
        var response = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucketName });
        return response.S3Objects.Select(obj => new S3Object { 
            FileName = Path.GetFileName(obj.Key),
            Key = obj.Key,
            Size = obj.Size,
            Extension = Path.GetExtension(obj.Key),
            CreatedAt = obj.LastModified, // Simpan sebagai DateTime
            LastModified = obj.LastModified // Simpan sebagai DateTime
            }).ToList();
    }

    public async Task<List<S3Object>> SearchObjectsAsync(string bucketName, string pathFileName)
    {
        var fullPrefix = $"{_urlPathKey}/{pathFileName}";
        var response = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucketName, Prefix = pathFileName });
        return response.S3Objects.Select(obj => new S3Object
        {
            FileName = Path.GetFileName(obj.Key),
            Key = obj.Key,
            Size = obj.Size,
            Extension = Path.GetExtension(obj.Key),
            CreatedAt = obj.LastModified, // Simpan sebagai DateTime
            LastModified = obj.LastModified // Simpan sebagai DateTime
        }).ToList();
    }

    public async Task<Stream> DownloadObjectAsync(string bucketName, string pathFileName)
    {
        var response = await s3Client.GetObjectAsync(new GetObjectRequest { BucketName = bucketName, Key = pathFileName });
        return response.ResponseStream;
    }
}