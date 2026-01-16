#nullable enable

namespace Toolbox.ValueObjects.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class EmailValueObjectTests
    {
        // -------------------------
        // POSITIVE CASES
        // -------------------------

        [Test]
        public void Create_With_Valid_Email_Succeeds()
        {
            const string email = "user@example.com";

            var result = Email.Create(email);

            Assert.That(result.Value, Is.EqualTo(email));
        }

        [Test]
        public void TryCreate_With_Valid_Email_Returns_True()
        {
            const string email = "john.doe+tag@sub.domain.org";

            var success = Email.TryCreate(email, out var result);

            Assert.That(success,      Is.True);
            Assert.That(result.Value, Is.EqualTo(email));
        }

        // -------------------------
        // NEGATIVE CASES
        // -------------------------

        [Test]
        public void Create_With_Null_Email_Throws()
        {
            Assert.That(
                (TestDelegate)(() => Email.Create(null!)),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Create_With_Empty_Email_Throws()
        {
            Assert.That(
                (TestDelegate)(() => Email.Create(string.Empty)),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Create_With_Whitespace_Email_Throws()
        {
            Assert.That(
                (TestDelegate)(() => Email.Create("   ")),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TryCreate_With_Null_Email_Returns_False()
        {
            var success = Email.TryCreate(null!, out var result);

            Assert.That(success, Is.False);
            Assert.That(result,  Is.EqualTo(default(Email)));
        }

        [Test]
        public void TryCreate_With_Empty_Email_Returns_False()
        {
            var success = Email.TryCreate(string.Empty, out var result);

            Assert.That(success, Is.False);
            Assert.That(result,  Is.EqualTo(default(Email)));
        }

        [Test]
        public void TryCreate_With_Whitespace_Email_Returns_False()
        {
            var success = Email.TryCreate("   ", out var result);

            Assert.That(success, Is.False);
            Assert.That(result,  Is.EqualTo(default(Email)));
        }

        // -------------------------
        // INVALID FORMAT CASES
        // -------------------------

        [TestCase("plainaddress")]
        [TestCase("missing-at-sign.com")]
        [TestCase("missing-domain@")]
        [TestCase("@missing-local-part.com")]
        [TestCase("user@.com")]
        [TestCase("user@domain")]
        [TestCase("user@domain.")]
        [TestCase("user@domain.c")]
        public void TryCreate_With_Invalid_Email_Format_Returns_False(string email)
        {
            var success = Email.TryCreate(email, out var result);

            Assert.That(success, Is.False);
            Assert.That(result,  Is.EqualTo(default(Email)));
        }

        [TestCase("plainaddress")]
        [TestCase("missing-at-sign.com")]
        public void Create_With_Invalid_Email_Format_Throws(string email)
        {
            Assert.That(
                (TestDelegate)(() => Email.Create(email)),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        // -------------------------
        // EDGE CASES
        // -------------------------

        [Test]
        public void Email_With_Subdomain_Is_Valid()
        {
            const string email = "user@mail.sub.domain.com";

            var success = Email.TryCreate(email, out var result);

            Assert.That(success,      Is.True);
            Assert.That(result.Value, Is.EqualTo(email));
        }

        [Test]
        public void Email_With_Plus_Addressing_Is_Valid()
        {
            const string email = "user+alias@gmail.com";

            var result = Email.Create(email);

            Assert.That(result.Value, Is.EqualTo(email));
        }

        // -------------------------
        // BEHAVIORAL GUARANTEES
        // -------------------------

        [Test]
        public void TryCreate_Does_Not_Throw_On_Invalid_Input()
        {
            Assert.That(
                (TestDelegate)(() => Email.TryCreate("not-an-email", out _)),
                Throws.Nothing);
        }

        [Test]
        public void Create_Throws_Even_When_Error_Message_Is_Null()
        {
            // error intentionally set to null in validator
            Assert.That(
                (TestDelegate)(() => Email.Create(" ")),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Message.Contains("Invalid Email value"));
        }
    }
}