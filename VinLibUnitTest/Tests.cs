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
            Assert.True(Vin.CheckVIN("JH4KB16535L011820"));
            Assert.True(Vin.CheckVIN("WBABJ7326WEA15710"));
            Assert.True(Vin.CheckVIN("KLAJB82Z2XK338143"));
            Assert.True(Vin.CheckVIN("WVWHV7AJ0AW084467"));
            Assert.False(Vin.CheckVIN("KLAVA6928XdB203010"));
            Assert.False(Vin.CheckVIN("ZZZVA6928XB203ZZZ"));
        }

        [Test]
        public void TransportCountryTest()
        {
            Assert.True(Vin.GetVINCountry("52111111111111111") == "США");
            Assert.True(Vin.GetVINCountry("TZ111111111111111") == "Португалия");
            Assert.True(Vin.GetVINCountry("89111111111111111") == "unknown country");
            Assert.True(Vin.GetVINCountry("TM111111111111111") == "Чехия");
        }
    }
}