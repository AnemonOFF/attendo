using Attendo.Infrastructure.Security;
using FluentAssertions;

namespace Attendo.Tests.Infrastructure;

public class Sha256PasswordHasherTests
{
    [Fact]
    public void Hash_ReturnsSaltedValueWithPrefix()
    {
        var hasher = new Sha256PasswordHasher();

        var result = hasher.Hash("P@ssw0rd!");

        result.Should().StartWith("SHA256$")
            .And.Contain("$");
        result.Split('$').Should().HaveCount(3);
    }

    [Fact]
    public void Hash_ProducesDifferentHashes_ForSamePassword()
    {
        var hasher = new Sha256PasswordHasher();

        var first = hasher.Hash("duplicate");
        var second = hasher.Hash("duplicate");

        first.Should().NotBe(second);
    }

    [Fact]
    public void Verify_ReturnsTrue_ForCorrectPassword()
    {
        var hasher = new Sha256PasswordHasher();
        var password = "Sup3rSecret!";
        var hash = hasher.Hash(password);

        hasher.Verify(password, hash).Should().BeTrue();
    }

    [Fact]
    public void Verify_ReturnsFalse_ForInvalidFormat()
    {
        var hasher = new Sha256PasswordHasher();

        hasher.Verify("anything", "not-a-valid-hash").Should().BeFalse();
    }
}

