using System;
using NUnit.Framework;
using REG_MARK_LIB;

namespace RegMarkLibTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CheckMarkTest()
        {
            Assert.True(RegMark.CheckMark("A111AA777"));
            Assert.True(RegMark.CheckMark("A252KH716"));
            Assert.False(RegMark.CheckMark("Q134JD788 "));
        }
    }
}