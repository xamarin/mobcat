using System.Threading;
using System.Threading.Tasks;
using Weather.Models;
using Weather.Services.Abstractions;

namespace Weather.Services
{
    public class ImageService
        : BaseWeatherService, IImageService
    {
        public ImageService(string apiKey)
            : base(apiKey)
        { }

        public async Task<string> GetImageAsync(string city, string weather, CancellationToken cancellationToken = default(CancellationToken))
        {
            var searchWeather = weather.Replace(" ", "+");
            var remoteImage = await GetAsync<CityWeatherImage>($"images/{city}?weather={searchWeather}", cancellationToken, SetApiKeyHeader).ConfigureAwait(false);

            return remoteImage.ImageUrl;
        }
    }
}