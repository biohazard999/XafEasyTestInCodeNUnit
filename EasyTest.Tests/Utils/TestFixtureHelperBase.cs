using System.Xml;
using DevExpress.EasyTest.Framework;

namespace EasyTest.Tests.Utils
{
    public abstract class TestFixtureHelperBase : IEasyTestFixtureHelper
    {
        public abstract TestCommandAdapter CommandAdapter { get; }
        public abstract ICommandAdapter Adapter { get; }
        public abstract bool IsWeb { get; }
        public abstract void SetUp();
        public abstract void SetupFixture();
        public abstract void TearDown();
        public abstract void TearDownFixture();

        protected static XmlAttribute CreateAttribute(XmlDocument doc, string attributeName, string attributeValue)
        {
            var entry = doc.CreateAttribute(attributeName);
            entry.Value = attributeValue;
            return entry;
        }

        protected static XmlAttribute CreateAttribute(XmlDocument doc, string attributeName, bool attributeValue)
            => CreateAttribute(doc, attributeName, attributeValue.ToString());
    }
}