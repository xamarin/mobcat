using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.MobCAT.Services
{
    public static class FileCache
    {
        public static async Task<string> SaveItemAsync<T>(string fileName, T item)
        {
            var storedFilepath = GetAbsolutePath(fileName);

            try
            {
                var itemJson = JsonConvert.SerializeObject(item);
                var encoding = new UTF8Encoding();
                var itemBytes = encoding.GetBytes(itemJson);

                using (var fileStream = new FileStream(storedFilepath, FileMode.Create, FileAccess.Write))
                {
                    await fileStream.WriteAsync(itemBytes, offset: 0, count: itemBytes.Length);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return storedFilepath;
        }

        public static async Task<T> LoadItemAsync<T>(string fileName)
        {
            var storedFilepath = GetAbsolutePath(fileName);

            if (File.Exists(storedFilepath))
            {
                try
                {
                    byte[] itemBytes = null;
                    using (var fileStream = new FileStream(storedFilepath, mode: FileMode.Open, access: FileAccess.Read))
                    {
                        itemBytes = new byte[fileStream.Length];
                        await fileStream.ReadAsync(itemBytes, offset: 0, count: Convert.ToInt32(fileStream.Length));
                        await fileStream.FlushAsync();
                    }

                    var encoding = new UTF8Encoding();
                    var itemJson = encoding.GetString(itemBytes);
                    var item = JsonConvert.DeserializeObject<T>(itemJson);
                    return item;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return default(T);
        }

        public static string GetAbsolutePath(string path)
        {
            var cacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (!path.StartsWith(cacheDirectory, StringComparison.Ordinal))
                return Path.Combine(cacheDirectory, path);

            return path;
        }
    }
}
