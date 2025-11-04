using System.Collections.Generic;
using System.Threading.Tasks;
using Appointments.Api.Controllers;
using Appointments.Core.Entities;
using Appointments.Core.DTOs;
using Appointments.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using FluentAssertions;
using System.Security.Cryptography;
using System.Text;

namespace Appointments.Tests.Controllers
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDb")
                .Options;

            var context = new AppDbContext(options);

            // Seed users
            context.Users.AddRange(new[]
            {
                new User { Username = "admin", PasswordHash = HashPassword("adminpass"), Role = Role.Admin },
                new User { Username = "user", PasswordHash = HashPassword("userpass"), Role = Role.User }
            });

            context.SaveChanges();
            return context;
        }

        private IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "ThisIsASecretKeyForTestingPurposesOnly"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:ExpireMinutes", "60"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task Login_ValidAdmin_ReturnsToken()
        {
            var context = GetInMemoryDbContext();
            var config = GetConfiguration();
            var controller = new AuthController(context, config);

            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "adminpass"
            };

            var result = await controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeAssignableTo<object>();
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsUnauthorized()
        {
            var context = GetInMemoryDbContext();
            var config = GetConfiguration();
            var controller = new AuthController(context, config);

            var loginDto = new LoginDto
            {
                Username = "wronguser",
                Password = "wrongpass"
            };

            var result = await controller.Login(loginDto);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsToken()
        {
            var context = GetInMemoryDbContext();
            var config = GetConfiguration();
            var controller = new AuthController(context, config);

            var loginDto = new LoginDto
            {
                Username = "user",
                Password = "userpass"
            };

            var result = await controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeAssignableTo<object>();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
