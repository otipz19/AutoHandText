namespace AutoHandTextTests
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void ContainsStringInStart()
        {
            string str = "aaa";
            string substring = "aaa";
            Assert.IsTrue(str.ContainsSubstring(substring));
        }

        [TestMethod]
        public void ContainsStringInStart1()
        {
            string str = "aaa";
            string substring = "a";
            Assert.IsTrue(str.ContainsSubstring(substring));
        }

        [TestMethod]
        public void ContainsStringInMiddle()
        {
            string str = "bbaabaaacc";
            string substring = "aaa";
            Assert.IsTrue(str.ContainsSubstring(substring));
        }

        [TestMethod]
        public void ContainsStringInEnd()
        {
            string str = "bbabaccaaa";
            string substring = "aaa";
            Assert.IsTrue(str.ContainsSubstring(substring));
        }

        [TestMethod]
        public void NotContainsString()
        {
            string str = "bbabaccaaa";
            string substring = "aaaaaaa";
            Assert.IsFalse(str.ContainsSubstring(substring));
        }
    }
}