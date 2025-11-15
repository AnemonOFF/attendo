using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Attendo.Domain.Entities;
using Attendo.Infrastructure.Auth;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Attendo.Tests.Infrastructure;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void CreateToken_EmbedsUserClaimsAndLifetime()
    {
        var config = BuildConfiguration(60);
        var generator = new JwtTokenGenerator(config);
        var user = new User
        {
            Id = 7,
            Login = "demo-user",
            Email = "demo@attendo.dev"
        };

        var expectedExpiry = DateTime.UtcNow.AddMinutes(60);

        var (token, expiresAt) = generator.CreateToken(user);

        expiresAt.Should().BeCloseTo(expectedExpiry, TimeSpan.FromSeconds(2));

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwt.Issuer.Should().Be("attendo-api");
        jwt.Audiences.Should().ContainSingle(a => a == "attendo-client");
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == user.Login);
        jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    }

    private static IConfiguration BuildConfiguration(double lifetimeMinutes)
    {
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = key,
            ["Jwt:Issuer"] = "attendo-api",
            ["Jwt:Audience"] = "attendo-client",
            ["Jwt:AccessTokenLifetimeMinutes"] = lifetimeMinutes.ToString("F0")
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}

