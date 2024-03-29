using System.Collections.Generic;
using System.IO;
using System.Xml;
using DevExpress.EasyTest.Framework;
using DevExpress.ExpressApp.EasyTest.WinAdapter;
using DevExpress.ExpressApp.Xpo;

namespace EasyTest.Tests.Utils
{
    public abstract class WinEasyTestFixtureHelperBase : TestFixtureHelperBase
    {
        private TestApplication application;
        private WinAdapter applicationAdapter;
        private string applicationDirectoryName;
        private string applicationName;
        protected ICommandAdapter adapter;
        protected TestCommandAdapter commandAdapter;

        public WinEasyTestFixtureHelperBase(string applicationDirectoryName, string applicationName)
        {
            this.applicationDirectoryName = applicationDirectoryName;
            this.applicationName = applicationName;
        }

        public override void SetUp()
        {
            applicationAdapter = new WinAdapter();
            applicationAdapter.RunApplication(application, $"ConnectionString={InMemoryDataStoreProvider.ConnectionString};FOO=BAR");
            adapter = ((IApplicationAdapter)applicationAdapter).CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter, application);
        }

        public override void SetupFixture()
        {
            application = new TestApplication();
            var doc = new XmlDocument();
            var additionalAttributes = new List<XmlAttribute>
            {
                CreateAttribute(doc, "FileName", Path.GetFullPath(Path.Combine($@"..\..\..\..\{applicationDirectoryName}", @"bin\EasyTest\net462\" + applicationName))),
                CreateAttribute(doc, "CommunicationPort", "4100"),
            };

            application.AdditionalAttributes = additionalAttributes.ToArray();
        }

        public override void TearDown()
            => applicationAdapter.KillApplication(application, KillApplicationContext.TestAborted);

        public override void TearDownFixture() { }

        public override ICommandAdapter Adapter => adapter;
        public override TestCommandAdapter CommandAdapter => commandAdapter;
        public override bool IsWeb => false;
    }
}
