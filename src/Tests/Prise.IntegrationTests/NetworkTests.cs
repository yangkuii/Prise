using System.Net.Http;
using System.Threading.Tasks;
using Prise.IntegrationTestsHost.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Prise.IntegrationTests
{
    public class NetworkTests : PluginTestBase,
         IClassFixture<AppHostWebApplicationFactory>
    {
        public NetworkTests(
                 AppHostWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task PluginCFromNetwork_Works()
        {
            // Arrange
            var payload = new CalculationRequestModel
            {
                A = 50,
                B = 2
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network", payload);

            // Assert 50 * 2 + 10% discount
            Assert.Equal(110, result.Result);
        }

        [Fact]
        public async Task PluginCFromNetwork_int_Works()
        {
            // Arrange
            var payload = new CalculationRequestModel
            {
                A = 50,
                B = 2
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network/int", payload);

            // Assert 50 * 2 + 10% discount
            Assert.Equal(110, result.Result);
        }


        [Fact]
        public async Task PluginCFromNetwork_complex_input_Works()
        {
            // Arrange
            var payload = new CalculationRequestModel
            {
                A = 50,
                B = 2
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network/complex-input", payload);

            // Assert 50 * 2 + 10% discount
            Assert.Equal(110, result.Result);
        }


        [Fact]
        public async Task PluginCFromNetwork_complex_output_Works()
        {
            // Arrange
            var payload = new CalculationRequestModel
            {
                A = 50,
                B = 2
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network/complex-output", payload);

            // Assert 50 * 2 + 10% discount
            Assert.Equal(110, result.Result);
        }

        [Fact]
        public async Task PluginCFromNetwork_multi_Works()
        {
            // Arrange
            var payload = new CalculationRequestMultiModel
            {
                Calculations = new[]
                {
                    new CalculationRequestModel
                    {
                        A = 50,
                        B = 2
                    },
                    new CalculationRequestModel
                    {
                        A = 40,
                        B = 2
                    }
                }
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network/multi", payload);

            // Assert (50 * 2 + 10% discount) + (40 * 2 + 10% discount)
            Assert.Equal(198, result.Result);
        }

        [Fact]
        public async Task PluginCFromNetwork_multi_async_Works()
        {
            // Arrange
            var payload = new CalculationRequestMultiModel
            {
                Calculations = new[]
                {
                    new CalculationRequestModel
                    {
                        A = 50,
                        B = 2
                    },
                    new CalculationRequestModel
                    {
                        A = 40,
                        B = 2
                    }
                }
            };

            //Act
            var result = await Post<CalculationResponseModel>(_client, "PluginCFromNetwork", "/network/multi-async", payload);

            // Assert (50 * 2 + 10% discount) + (40 * 2 + 10% discount)
            Assert.Equal(198, result.Result);
        }
    }
}
