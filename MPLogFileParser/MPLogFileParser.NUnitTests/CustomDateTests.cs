namespace MPLogFileParser.NUnitTests
{
    public class CustomDateTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CustDateTime_SetDateTime_InvalidFormats()
        {
            CustomDateTime dateTime = new CustomDateTime();
            Assert.IsFalse(dateTime.SetDateTime("23:12:01"));// not enough #s
            Assert.IsFalse(dateTime.SetDateTime("23:12:01:01:01")); //too many #s
            Assert.IsFalse(dateTime.SetDateTime("23:12:01:a")); //invalid char
            Assert.IsFalse(dateTime.SetDateTime("23:12:b2:12")); //invalid char
            Assert.IsFalse(dateTime.SetDateTime("32:12:12:12")); //invalid #
            Assert.IsFalse(dateTime.SetDateTime("0:12:12:12")); //invalid #
            Assert.IsFalse(dateTime.SetDateTime("12:-2:12:12")); //invalid #
            Assert.IsFalse(dateTime.SetDateTime("12:12:61:12")); //invalid #
            Assert.IsFalse(dateTime.SetDateTime("12:12:12:60")); //invalid #
            Assert.IsFalse(dateTime.SetDateTime("[12:12:61:12]")); //invalid chars

        }
        [Test]
        public void CustDateTime_SetDateTime_ValidFormats()
        {
            CustomDateTime dateTime = new CustomDateTime();
            Assert.IsTrue(dateTime.SetDateTime("23:12:01:59"));
            Assert.IsTrue(dateTime.SetDateTime("31:23:59:59")); //highest #s
            Assert.IsTrue(dateTime.SetDateTime("01:00:00:00")); //lowest #s

        }
    }
}