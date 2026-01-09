using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;


namespace GTracker.IntegrationTests;

    public class IndexTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IndexTests(CustomWebApplicationFactory factory)
        {
            // keep cookies/sessions
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Index_WhenNotLoggedIn_RedirectsToLogin()
        {
            var resp = await _client.GetAsync("/Index");
            Assert.Equal(HttpStatusCode.Redirect, resp.StatusCode);
            Assert.Contains("/Login", resp.Headers.Location!.ToString());
        }

        [Fact]
        public async Task Index_WhenLoggedIn_ReturnsOk()
        {
            TestDb.Reset();
            var userId = TestDb.InsertUser();

            var login = await _client.GetAsync($"/test-login/{userId}");
            Assert.Equal(HttpStatusCode.OK, login.StatusCode);

            var resp = await _client.GetAsync("/Index");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }
    }

