
using System;
using System.Net;
using System.Threading.Tasks;
using Gift_of_the_Givers.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task DonatePage_ShouldLoadSuccessfully()
        {
            // ARRANGE
            // Start the web application using a temporary test database.

            using var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");

                    builder.ConfigureServices(services =>
                    {
                        // Remove the original SQL Server database configuration.
                        services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

                        // Configure an in-memory database for testing.
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase(
                                "GiftOfTheGiversIntegrationTest"));
                    });
                });

            using var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // ACT
            // Send an HTTP request to the donation page.
            var response = await client.GetAsync("/Donate");

            // ASSERT
            // Confirm that the donation page loads successfully.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
