using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Core.Tests
{
    public class PhoneNumberNormalizer
    {
        private const string ItalyPrefix = "+39";

        public string Normalize(string phoneNumber)
        {
            string trimmedPhoneNumber = phoneNumber.Replace(" ", "");
            return trimmedPhoneNumber.StartsWith(ItalyPrefix)
                ? trimmedPhoneNumber.Substring(1)
                : trimmedPhoneNumber.Length == 10
                    ? $"39{trimmedPhoneNumber}"
                    : trimmedPhoneNumber;
        }
    }

    [TestFixture()]
    class PhoneNumberNormalizerTests
    {
        [TestCase("3479686512", "393479686512")]
        [TestCase("+393479686512", "393479686512")]
        [TestCase("+39 3479686512", "393479686512")]
        [TestCase("347 968 6512", "393479686512")]
        [TestCase("347 968  6512", "393479686512")]
        [TestCase("+39 347 968 6512", "393479686512")]
        [TestCase("+39 347 9686512", "393479686512")]
        public void TestPhoneNumberNormalization(string input, string expected)
        {
            PhoneNumberNormalizer phoneNumberNormalizer = new PhoneNumberNormalizer();
            string actual = phoneNumberNormalizer.Normalize(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
