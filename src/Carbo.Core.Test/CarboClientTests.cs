﻿using Carbo.Core.Client;
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
            CarboUrl url = CarboUrl.Create("https://catfact.ninja/fact", [], []);
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = url,
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
                ClientTimeout = TimeSpan.FromMinutes(1),
            };

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test to send an invalid request with request error response.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendRequest_WithInvalidRequest_ReturnsResponseWithRequestError()
        {
            // Arrange
            CarboUrl randomUrl = CarboUrl.Create($"https://{Guid.NewGuid():N}/{Guid.NewGuid():N}/{Guid.NewGuid():N}", [], []);
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = randomUrl,
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
                ClientTimeout = TimeSpan.FromMinutes(1),
            };

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.RequestError);
        }

        /// <summary>
        /// Test to send a request with expected exceeded client timeout.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendRequest_WithInvalidRequest_ReturnsResponseWithExceededClientTimeout()
        {
            // Arrange
            // See https://stackoverflow.com/a/14503574
            CarboUrl timeoutURL = CarboUrl.Create($"http://www.google.com:81/", [], []);
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = timeoutURL,
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
                // The client timeout is set to 1 tick, the minimum value accepted by the inner http client implementation.
                // This should force the client timeout to occur.
                ClientTimeout = TimeSpan.FromTicks(1),
            };

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.ExceededClientTimeout);
            Assert.True(response.ElapsedTime > TimeSpan.Zero);
        }

        /// <summary>
        /// Test to send a request with expected exceeded socket timeout error.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendRequest_WithInvalidRequest_ReturnsResponseWithExceededSocketTimeoutError()
        {
            // Arrange
            // See https://stackoverflow.com/a/14503574
            CarboUrl timeoutURL = CarboUrl.Create($"http://www.google.com:81/", [], []);
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = timeoutURL,
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
                // The client timeout is set to the maximum value accepted by the inner http client implementation.
                // Thi should force the socket timeout to occur.
                ClientTimeout = Timeout.InfiniteTimeSpan,
            };

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.ExceededClientTimeout);
            Assert.True(response.ElapsedTime > TimeSpan.Zero);
            Assert.NotNull(response.SocketError);
        }

        /// <summary>
        /// Test to send a request with a valid request and cancellation token.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendRequest_WithValidRequestAndCancellationToken_ReturnsResponse()
        {
            // Arrange
            CarboUrl url = CarboUrl.Create("https://catfact.ninja/fact", [], []);
            CarboRequest request = new()
            {
                HttpMethod = HttpMethod.Get,
                Url = url,
                Headers =
                [
                    new CarboKeyValuePair { Key = "Accept", Value = "application/json" }
                ],
                ClientTimeout = TimeSpan.FromMinutes(1),
            };

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();

            // Act
            CarboResponse response = await CarboClient.Instance.SendRequestAsync(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.ExceededClientTimeout);
        }
    }
}
