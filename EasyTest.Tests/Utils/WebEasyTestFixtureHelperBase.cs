using System;
using System.IO;
using DevExpress.EasyTest.Framework;
using System.Reflection;
using DevExpress.ExpressApp.Xpo;
using System.Xml;
using System.Collections.Generic;

namespace EasyTest.Tests.Utils
{
    public abstract class WebEasyTestFixtureHelperBase : IEasyTestFixtureHelper
    {
        private const string testWebApplicationRootUrl = "http://localhost:3057";
        protected TestWebAdapter webAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        protected TestApplication application;
        public WebEasyTestFixtureHelperBase(string relativePathToWebApplication)
        {
            string testApplicationDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePathToWebApplication);

            application = new TestApplication
            {
                IgnoreCase = true,
            };
            
            List<XmlAttribute> additionalAttributes = new List<XmlAttribute>();
            XmlDocument doc = new XmlDocument();
            XmlAttribute entry = doc.CreateAttribute("PhysicalPath");
            entry.Value = testApplicationDir;
            additionalAttributes.Add(entry);
            entry = doc.CreateAttribute("URL");
            entry.Value = testWebApplicationRootUrl + GetUrlOptions();
            additionalAttributes.Add(entry);

            entry = doc.CreateAttribute("SingleWebDev");
            entry.Value = true.ToString();
            additionalAttributes.Add(entry);
            
            XmlAttribute entry2 = doc.CreateAttribute("DontRestartIIS");
            entry2.Value = true.ToString();
            additionalAttributes.Add(entry2);

            XmlAttribute entry3 = doc.CreateAttribute("UseIISExpress");
            entry3.Value = true.ToString();
            additionalAttributes.Add(entry3);

            application.AdditionalAttributes = additionalAttributes.ToArray();
        }
        protected virtual string GetUrlOptions()
        {
            return "/default.aspx";
        }
        public virtual void SetupFixture()
        {
            webAdapter = new TestWebAdapter();
            webAdapter.RunApplication(application, InMemoryDataStoreProvider.ConnectionString);
        }
        public virtual void SetUp()
        {
            adapter = webAdapter.CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter, application);
        }
        public virtual void TearDown()
        {
            string urlParams = GetUrlOptions();
            webAdapter.WebBrowser.Navigate(testWebApplicationRootUrl + urlParams + (urlParams.Contains("?") ? "&" : "?") + "Reset=true");
        }
        public virtual void TearDownFixture()
        {
            webAdapter.WebBrowser.Close();
            webAdapter.KillApplication(application, KillApplicationContext.TestAborted);
        }
        public TestCommandAdapter CommandAdapter
        {
            get
            {
                return commandAdapter;
            }
        }
        public ICommandAdapter Adapter
        {
            get
            {
                return adapter;
            }
        }

        public bool IsWeb => true;
    }
}