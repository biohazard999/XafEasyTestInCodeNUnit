using System;
using System.IO;
using DevExpress.EasyTest.Framework;
using System.Reflection;

namespace EasyTest.Tests.Utils
{
    public abstract class WebEasyTestFixtureHelperBase : IEasyTestFixtureHelper
    {
        private const string testWebApplicationRootUrl = "http://localhost:3057";
        protected TestWebAdapter webAdapter;
        protected TestCommandAdapter commandAdapter;
        protected ICommandAdapter adapter;
        protected TestApplication application;
        public WebEasyTestFixtureHelperBase(string relativePathToWebApplication, string absolutePathToWebApplication)
        {
            string testApplicationDir = Path.Combine(Assembly.GetExecutingAssembly().Location, relativePathToWebApplication);
            if (!Directory.Exists(testApplicationDir))
                testApplicationDir = absolutePathToWebApplication;
            //string.Empty, testApplicationDir, testWebApplicationRootUrl + GetUrlOptions(), string.Empty
            application = new TestApplication()
            {
                Name = string.Empty,
            };
            //application.AddParam("SingleWebDev", true.ToString());
        }
        protected virtual string GetUrlOptions()
        {
            return "/default.aspx";
        }
        public virtual void SetupFixture()
        {
            webAdapter = new TestWebAdapter();
            webAdapter.RunApplication(application, null);
        }
        public virtual void SetUp()
        {
            adapter = webAdapter.CreateCommandAdapter();
            commandAdapter = new TestCommandAdapter(adapter, application);
        }
        public virtual void TearDown()
        {
            webAdapter.WebBrowser.Close();

            //string urlParams = GetUrlOptions();
            //webAdapter.WebBrowsers[0].Navigate(testWebApplicationRootUrl + urlParams + (urlParams.Contains("?") ? "&" : "?") + "Reset=true");
        }
        public virtual void TearDownFixture()
        {

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
    }
}