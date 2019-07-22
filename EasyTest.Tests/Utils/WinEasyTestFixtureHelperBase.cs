using System;
using DevExpress.ExpressApp.EasyTest.WinAdapter;
using System.IO;
using DevExpress.EasyTest.Framework;
using DevExpress.ExpressApp.Xpo;
using System.Collections.Generic;
using System.Xml;

namespace EasyTest.Tests.Utils {
    public abstract class WinEasyTestFixtureHelperBase : IEasyTestFixtureHelper {
        private string applicationDirectoryName;
        private string applicationName;
        private TestApplication application;
        private WinAdapter applicationAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        public WinEasyTestFixtureHelperBase(string applicationDirectoryName, string applicationName) {
            this.applicationDirectoryName = applicationDirectoryName;
            this.applicationName = applicationName;
        }
        public void SetupFixture() {
            application = new TestApplication();
            List<XmlAttribute> additionalAttributes = new List<XmlAttribute>();
            XmlDocument doc = new XmlDocument();
            XmlAttribute entry = doc.CreateAttribute("FileName");
            entry.Value = Path.GetFullPath(Path.Combine(@"..\..\..\" + applicationDirectoryName, @"Bin\EasyTest\" + applicationName));
            additionalAttributes.Add(entry);
            entry = doc.CreateAttribute("CommunicationPort");
            entry.Value = "4100";
            additionalAttributes.Add(entry);
            application.AdditionalAttributes = additionalAttributes.ToArray();
        }
        public void SetUp() {
            applicationAdapter = new WinAdapter();
            applicationAdapter.RunApplication(application, InMemoryDataStoreProvider.ConnectionString);
            adapter = ((IApplicationAdapter)applicationAdapter).CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter, application);
        }
        public void TearDown() {
            applicationAdapter.KillApplication(application, KillApplicationContext.TestAborted);
        }
        public void TearDownFixture() {
        }
        public TestCommandAdapter CommandAdapter {
            get {
                return commandAdapter;
            }
        }
        public ICommandAdapter Adapter {
            get {
                return adapter;
            }
        }
    }
}
