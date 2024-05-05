using Carbo.Core.Client;
using Carbo.Core.Models.Http;
using System.Net;
using Xunit;

namespace Carbo.Core.Test
{
    /// <summary>
    /// Test class for CarboClient.
    /// </summary>
    public class CarboClientTests
    {
        /// <summary>
        /// Test to send a valid request.
        /// </summary>
        [Fact]
        public async Task SendRequest_WithValidRequest_ReturnsResponse()
        {
            // Arrange
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = new Uri("https://catfact.ninja/fact"),
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
            };

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
