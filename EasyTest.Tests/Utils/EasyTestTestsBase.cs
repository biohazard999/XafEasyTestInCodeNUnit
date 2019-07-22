using System;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;

namespace EasyTest.Tests.Utils {
    [TestFixture]
    public class EasyTestTestsBase<T> where T : IEasyTestFixtureHelper, new() {
        private IEasyTestFixtureHelper helper;
        protected TestCommandAdapter commandAdapter {
            get {
                return helper.CommandAdapter;
            }
        }
        protected ICommandAdapter adapter {
            get {
                return helper.Adapter;
            }
        }
        [OneTimeSetUp]
        public void SetupFixture() {
            helper = new T();
            helper.SetupFixture();
        }
        [SetUp]
        public void SetUp() {
            helper.SetUp();
        }
        [TearDown]
        public void TearDown() {
            helper.TearDown();
        }
        [OneTimeTearDown]
        public void TearDownFixture() {
            helper.TearDownFixture();
        }
    }
}
