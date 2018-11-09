using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Weather.Services.Abstractions;

namespace Weather.Services
{
    public static class WebImageCache
    {
        static HttpClient downloadClient = new HttpClient();

        public static async Task<string> RetrieveImage(string imageUrl, string fileName = null)
        {
            byte[] imageBytes;
            var storedFilename = string.IsNullOrEmpty(fileName) ? imageUrl : fileName;
            var cacheStorage = ServiceContainer.Resolve<IFileStorageService>(true);

            if (cacheStorage == null)
                return imageUrl;

            if (!cacheStorage.FileExists(storedFilename))
            {

                try
                {
                    imageBytes = await downloadClient.GetByteArrayAsync(new Uri(imageUrl));

                    using (var stream = cacheStorage.CreateOutputStream(storedFilename))
                    {
                        await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                        await stream.FlushAsync();

                        stream.Dispose();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return $"{cacheStorage.BasePath}/{storedFilename}";
        }
    }
}