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
            Assert.False(RegMark.CheckMark("Q134JD788"));
            Assert.True(RegMark.CheckMark("B777XX777"));
            Assert.True(RegMark.CheckMark("A000AB716"));
            Assert.False(RegMark.CheckMark("A123BC000"));
        }

        [Test]
        public void GetNextMarkAfterTest()
        {
            Assert.True(RegMark.GetNextMarkAfter("A123AA716") == "A124AA716");
            Assert.True(RegMark.GetNextMarkAfter("A999AA716") == "A000AB716");
            Assert.True(RegMark.GetNextMarkAfter("A999XX716") == "B000AA716");
            Assert.True(RegMark.GetNextMarkAfter("B777XX777") == "B778XX777");
        }
    }
}