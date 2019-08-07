using DevExpress.EasyTest.Framework;
using DevExpress.ExpressApp.EasyTest.WebAdapter;
using DevExpress.ExpressApp.Xpo;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace EasyTest.Tests.Utils
{
    public abstract class WebEasyTestFixtureHelperBase : TestFixtureHelperBase
    {
        private const string testWebApplicationRootUrl = "http://localhost:3057";
        protected WebAdapter webAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        protected TestApplication application;
        public WebEasyTestFixtureHelperBase(string relativePathToWebApplication)
        {
            var testApplicationDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePathToWebApplication);

            application = new TestApplication
            {
                IgnoreCase = true,
            };

            var doc = new XmlDocument();
            
            var additionalAttributes = new List<XmlAttribute>
            {
                CreateAttribute(doc, "PhysicalPath", testApplicationDir),
                CreateAttribute(doc, "URL", $"{testWebApplicationRootUrl}{GetUrlOptions()}"),
                CreateAttribute(doc, "SingleWebDev", true),
                CreateAttribute(doc, "DontRestartIIS", true),
                CreateAttribute(doc, "UseIISExpress", true),
            };
            
            application.AdditionalAttributes = additionalAttributes.ToArray();
        }
        
        protected virtual string GetUrlOptions() => "/default.aspx";

        public override void SetupFixture()
        {
            webAdapter = new WebAdapter();
            webAdapter.RunApplication(application, InMemoryDataStoreProvider.ConnectionString);
        }
        
        public override void SetUp()
        {
            adapter = webAdapter.CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter, application);
        }
        
        public override void TearDown()
        {
            var urlParams = GetUrlOptions();
            webAdapter.WebBrowser.Navigate(testWebApplicationRootUrl + urlParams + (urlParams.Contains("?") ? "&" : "?") + "Reset=true");
        }
        
        public override void TearDownFixture()
        {
            webAdapter.WebBrowser.Close();
            webAdapter.KillApplication(application, KillApplicationContext.TestAborted);
        }
        
        public override TestCommandAdapter CommandAdapter => commandAdapter;
        public override ICommandAdapter Adapter => adapter;
        public override bool IsWeb => true;
    }
}