namespace AmazonAsinTracker.Infrastructure.FileStorage
{
    public class FileStorageProvider : IFileStorageProvider
    {
        private readonly string _folderLocation;

        public FileStorageProvider(string folderLocation)
        {
            _folderLocation = folderLocation;
        }

        public string GetFolderLocation() => _folderLocation;
    }
}