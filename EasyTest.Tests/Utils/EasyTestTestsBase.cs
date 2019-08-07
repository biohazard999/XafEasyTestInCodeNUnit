using DevExpress.EasyTest.Framework;
using NUnit.Framework;

namespace EasyTest.Tests.Utils
{
    [TestFixture]
    public class EasyTestTestsBase<T> where T : IEasyTestFixtureHelper, new()
    {
        private IEasyTestFixtureHelper helper;
        protected TestCommandAdapter commandAdapter => helper.CommandAdapter;
        protected ICommandAdapter adapter => helper.Adapter;
        
        [OneTimeSetUp]
        public void SetupFixture()
        {
            helper = new T();
            helper.SetupFixture();
        }
        
        [SetUp]
        public void SetUp() => helper.SetUp();

        [TearDown]
        public void TearDown() => helper.TearDown();

        [OneTimeTearDown]
        public void TearDownFixture() => helper.TearDownFixture();

        protected bool IsWeb => helper.IsWeb;
    }
}
