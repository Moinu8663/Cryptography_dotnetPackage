using System;
using System.Security.Cryptography;
using Cryptography;
using FluentAssertions;
using Xunit;

namespace Cryptographytest
{
    public class CryptographyTests
    {
        [Fact]
        public void DoubleEncryptAndDoubleDecrypt_String_RoundTrips()
        {
            var crypto = new Cryptography.Cryptography("master-key");

            var input = "hello world";
            var encrypted = crypto.DoubleEncrypt(input);
            var decrypted = crypto.DoubleDecrypt<string>(encrypted);

            decrypted.Should().Be(input);
        }

        [Fact]
        public void DoubleEncryptAndDoubleDecrypt_Int_RoundTrips()
        {
            var crypto = new Cryptography.Cryptography("master-key");

            var input = 12345;
            var encrypted = crypto.DoubleEncrypt(input);
            var decrypted = crypto.DoubleDecrypt<int>(encrypted);

            decrypted.Should().Be(input);
        }

        [Fact]
        public void DoubleEncryptAndDoubleDecrypt_Object_RoundTrips()
        {
            var crypto = new Cryptography.Cryptography("master-key");

            var person = new Person { Name = "Alice", Age = 30 };
            var encrypted = crypto.DoubleEncrypt(person);
            var decrypted = crypto.DoubleDecrypt<Person>(encrypted);

            decrypted.Should().BeEquivalentTo(person);
        }

        [Fact]
        public void DoubleEncrypt_SamePlaintext_ProducesDifferentCiphertexts()
        {
            var crypto = new Cryptography.Cryptography("master-key");

            var input = "repeatable";
            var first = crypto.DoubleEncrypt(input);
            var second = crypto.DoubleEncrypt(input);

            first.Should().NotBe(second);
        }

        [Fact]
        public void DoubleDecrypt_WithWrongMasterKey_ThrowsCryptographicException()
        {
            var good = new Cryptography.Cryptography("good-key");
            var bad = new Cryptography.Cryptography("bad-key");

            var encrypted = good.DoubleEncrypt("secret");

            Action act = () => bad.DoubleDecrypt<string>(encrypted);

            act.Should().Throw<CryptographicException>();
        }
    }

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
