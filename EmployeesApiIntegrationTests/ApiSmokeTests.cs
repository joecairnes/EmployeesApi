using EmployeesApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeesApiIntegrationTests
{
    public class ApiSmokeTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ApiSmokeTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/books")]
        [InlineData("/whoami")]
        [InlineData("/time")]
        public async Task ResourcesAreAlive(string url)
        {
            // Given (arrange)
            var client = _factory.CreateClient();

            // When (act)
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

    }
}
