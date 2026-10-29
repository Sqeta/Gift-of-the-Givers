
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Gift_of_the_Givers.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace GiftOfTheGivers.Tests
{
    public class LoadTests
    {
        private readonly ITestOutputHelper _output;

        public LoadTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public async Task DonationPage_ShouldHandleConcurrentRequests(
            int numberOfUsers)
        {
            // ==========================================
            // ARRANGE - TEST APPLICATION
            // ==========================================

            using var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");

                    builder.ConfigureServices(services =>
                    {
                        // Remove SQL Server database configuration.
                        services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

                        // Create a temporary in-memory database.
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase(
                                $"LoadTestDatabase_{Guid.NewGuid()}"));
                    });
                });

            using var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Warm up the application before testing.
            using var warmup = await client.GetAsync("/Donate");

            Assert.Equal(HttpStatusCode.OK, warmup.StatusCode);

            // ==========================================
            // ACT - SIMULATE CONCURRENT REQUESTS
            // ==========================================

            var stopwatch = Stopwatch.StartNew();

            var requests = Enumerable.Range(0, numberOfUsers)
                .Select(_ => client.GetAsync("/Donate"))
                .ToArray();

            var responses = await Task.WhenAll(requests);

            stopwatch.Stop();

            // ==========================================
            // ASSERT - CHECK RESPONSES
            // ==========================================

            int successfulRequests = responses.Count(
                response => response.StatusCode == HttpStatusCode.OK);

            int failedRequests = numberOfUsers - successfulRequests;

            Assert.Equal(numberOfUsers, successfulRequests);

            // ==========================================
            // PERFORMANCE RESULTS
            // ==========================================

            double totalSeconds = stopwatch.Elapsed.TotalSeconds;

            double throughput = totalSeconds > 0
                ? numberOfUsers / totalSeconds
                : 0;

            _output.WriteLine("====================================");
            _output.WriteLine("GIFT OF THE GIVERS - LOAD TEST");
            _output.WriteLine("====================================");

            _output.WriteLine(
                $"Concurrent requests: {numberOfUsers}");

            _output.WriteLine(
                $"Successful requests: {successfulRequests}");

            _output.WriteLine(
                $"Failed requests: {failedRequests}");

            _output.WriteLine(
                $"Total duration: {stopwatch.ElapsedMilliseconds} ms");

            _output.WriteLine(
                $"Average throughput: {throughput:F2} requests/second");

            _output.WriteLine("====================================");

            // Release HTTP responses.
            foreach (var response in responses)
            {
                response.Dispose();
            }
        }
    }
}
