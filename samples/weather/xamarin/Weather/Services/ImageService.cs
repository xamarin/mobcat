using System.Threading;
using System.Threading.Tasks;
using Weather.Models;
using Weather.Services.Abstractions;

namespace Weather.Services
{
    public class ImageService
        : BaseWeatherService, IImageService
    {
        public ImageService(string apiBaseAddress, string apiKey)
            : base(apiBaseAddress, apiKey)
        { }

        public async Task<string> GetImageAsync(string city, string weather, CancellationToken cancellationToken = default(CancellationToken))
        {
            var searchWeather = weather.Replace(" ", "+");
            var remoteImage = await GetAsync<CityWeatherImage>($"images/{city}?weather={searchWeather}", cancellationToken, SetApiKeyHeader).ConfigureAwait(false);

            return await WebImageCache.RetrieveImage(remoteImage.ImageUrl, $"{city}-{searchWeather}");
        }
    }
}