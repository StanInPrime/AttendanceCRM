namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            //setup data needed or resources needed to conduct the tests
        }
        //behave as a normal method
        [Test]
        public void Test1()
        {
            Assert.Pass();
            //this is an assertion, commonly used to verify if the condition is true or false
        }
    }
}