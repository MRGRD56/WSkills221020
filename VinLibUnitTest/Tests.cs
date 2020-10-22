using System;
using NUnit.Framework;
using VIN_LIB;

namespace VinLibUnitTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TransportYearTest() 
        {
            Assert.True(Vin.GetTransportYear("111111111F1111111") == 2015);
            Assert.True(Vin.GetTransportYear("111111111M1111111") == 1991);
            Assert.True(Vin.GetTransportYear("111111111L1111111") == 2020);
        }

        [Test]
        public void VimCorrectnessTest()
        {
            Assert.False(Vin.CheckVIN("1234567890ZQROOI0"));
            Assert.True(Vin.CheckVIN("12340723423423423"));
            Assert.False(Vin.CheckVIN("123407234234234X3"));
        }

        [Test]
        public void TransportCountryTest()
        {
            var country1 = Vin.GetVINCountry("52111111111111111");
            Assert.True(country1 == "США");
            Assert.True(Vin.GetVINCountry("TZ111111111111111") == "Португалия");
        }
    }
}