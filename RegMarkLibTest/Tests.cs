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

        [Test]
        public void GetNextMarkAfterInRangeTest()
        {
            Assert.True(RegMark.GetNextMarkAfterInRange("A105AA716", "A100AA716", "A110AA716") == "A106AA716");
            Assert.True(RegMark.GetNextMarkAfterInRange("A110AA716", "A100AA716", "A110AA716") == "out of stock");
            Assert.True(RegMark.GetNextMarkAfterInRange("A999AA716", "A100AA716", "A006AB716") == "A000AB716");
            Assert.True(RegMark.GetNextMarkAfterInRange("B999XX116", "B000XX116", "C010AA116") == "E000AA116");
        }

        [Test]
        public void GetCombinationsCountInRangeTest()
        {
            Assert.True(RegMark.GetCombinationsCountInRange("A123AA716", "A125AA716") == 3);
            Assert.True(RegMark.GetCombinationsCountInRange("A999AA716", "A010AB716") == 12);
            Assert.True(RegMark.GetCombinationsCountInRange("B567AA116", "B568AA116") == 2);
            Assert.True(RegMark.GetCombinationsCountInRange("A999XX116", "B010AA116") == 12);
        }
    }
}