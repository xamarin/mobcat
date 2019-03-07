using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.Services
{
    public static class WebImageCache
    {
        static HttpClient downloadClient = new HttpClient();

        public static async Task<string> RetrieveImage(string imageUrl, string fileName = null)
        {
            byte[] imageBytes;
            var storedFilename = string.IsNullOrEmpty(fileName) ? imageUrl : fileName;
            var storedFilepath = GetAbsolutePath(storedFilename);

            if (!File.Exists(storedFilepath))
            {
                try
                {
                    imageBytes = await downloadClient.GetByteArrayAsync(new Uri(imageUrl));

                    using (var stream = new FileStream(storedFilepath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                        await stream.FlushAsync();

                        stream.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            return storedFilepath;
        }

        public static string GetAbsolutePath(string path)
        {
            var cacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);

            if (string.IsNullOrEmpty(cacheDirectory))
            {
                cacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }

            if (!path.StartsWith(cacheDirectory, StringComparison.Ordinal))
                return Path.Combine(cacheDirectory, path);

            return path;
        }
    }
}