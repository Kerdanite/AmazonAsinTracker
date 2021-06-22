using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;
using AmazonAsinTracker.Infrastructure.FileStorage;

namespace AmazonAsinTracker.Infrastructure
{
    public class ProductAsinFileRepository : IProductAsinRepository
    {
        private readonly IFileStorageProvider _fileStorageProvider;
        private readonly string _productToTrackFiles;
        private char fileSeparator = ';';

        public ProductAsinFileRepository(IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
            _productToTrackFiles = _fileStorageProvider.GetFolderLocation() + "/productToTrack.txt";
        }

        public async Task TrackProductsByAsinCodeAsync(IEnumerable<string> requestProductAsins, CancellationToken cancellationToken)
        {
            if (File.Exists(_productToTrackFiles))
            {
                File.Delete(_productToTrackFiles);
            }

            using (StreamWriter sw = File.CreateText(_productToTrackFiles))
            {
                await sw.WriteLineAsync(string.Join(fileSeparator, requestProductAsins));
            }	
        }

        public async Task<IEnumerable<string>> GetProductAsinToTrack(CancellationToken cancellationToken)
        {
            if (!File.Exists(_productToTrackFiles))
            {
                return new List<string>();
            }
            using (StreamReader sr = File.OpenText(_productToTrackFiles))
            {
                string content = (await sr.ReadToEndAsync()).Trim();
                return content.Split(fileSeparator);
            }
        }
    }
}