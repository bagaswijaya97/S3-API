public class S3Object
{
    public string FileName { get; set; }
    public string Key { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}