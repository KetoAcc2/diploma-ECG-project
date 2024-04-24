/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void Shuffle_WhenSeedIs132_ShuffledListShouldBeEqualToExpected()
        {
            const int SEED = 132;
            List<string> list = new List<string>(5);
            for (int i = 0; i < list.Capacity; i++)
            {
                list.Add(i.ToString());
            }

            var oldList = new List<string>(list);
            var shuffledList = new List<string>(list);

            Utils.Shuffle(shuffledList, SEED);

            Assert.AreNotEqual(oldList, shuffledList, "Are equal!!");

            var expected = new List<string>() {
                oldList[4], //0
                oldList[1], //1
                oldList[0], //2
                oldList[2], //3
                oldList[3], //4
            };

            CollectionAssert.AreEqual(expected, shuffledList, $"");
        }

        [TestMethod]
        public void getAccessToken_ProvidedTokenString_ReturnedStringShouldBeEqualToExpected()
        {
            var str = "Bearer YAXS232YHG21W8";
            var expected = "YAXS232YHG21W8";
            var returned = Utils.getAccessToken(str);

            Assert.AreEqual(expected, returned);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void getAccessToken_ProvidedEmptyString_ThrowsArgumentNullException()
        {
            Utils.getAccessToken(null);
        }
    }

}
*/