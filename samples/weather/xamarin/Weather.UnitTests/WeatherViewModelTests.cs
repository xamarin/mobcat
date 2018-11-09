using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Moq;
using Weather.Models;
using Weather.Services;
using Weather.Services.Abstractions;
using Weather.ViewModels;
using Xamarin.Essentials;
using Xunit;

namespace Weather.UnitTests
{
    public class WeatherViewModelTests
    {
        [Fact]
        public async void TestInit()
        {
            //Mock the forecast service
            var mockForecastService = new Mock<IForecastsService>();
            mockForecastService.Setup(a => a.GetForecastAsync(It.IsAny<string>(), It.IsAny<TemperatureUnit>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(
                new Forecast
                {
                    Id = "MockForecast",
                    CurrentTemperature = "900",
                    Description = "It's way too hot",
                    MinTemperature = "800",
                    MaxTemperature = "1000",
                    Name = "Forecast Name",
                    Overview = "Forecast Overview"
                }));

            ServiceContainer.Register<IForecastsService>(mockForecastService.Object);

            //Mock the image service
            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(a => a.GetImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult("Image URL"));

            ServiceContainer.Register<IImageService>(mockImageService.Object);


            //Init the VM
            var weatherViewModel = new WeatherViewModel();
            await weatherViewModel.InitAsync();

            //Get expecteds for asserts
            var expectedImage = await mockImageService.Object.GetImageAsync(default(string), default(String), default(CancellationToken));
            var expectedForecast = await mockForecastService.Object.GetForecastAsync(default(string), default(TemperatureUnit), default(CancellationToken));

            //Assert
            Assert.Equal(expectedForecast.Overview, weatherViewModel.WeatherDescription);
            Assert.Equal(expectedForecast.CurrentTemperature, weatherViewModel.CurrentTemp);
            Assert.Equal(expectedForecast.MaxTemperature, weatherViewModel.HighTemp);
            Assert.Equal(expectedForecast.MinTemperature, weatherViewModel.LowTemp);
            Assert.Equal(expectedImage, weatherViewModel.BackgroundImage);
        }
    }
}
