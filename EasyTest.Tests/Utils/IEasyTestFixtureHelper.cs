using System;
using DevExpress.EasyTest.Framework;

namespace EasyTest.Tests.Utils {
    public interface IEasyTestFixtureHelper {
        void SetupFixture();
        void SetUp();
        void TearDown();
        void TearDownFixture();
        TestCommandAdapter CommandAdapter { get; }
        ICommandAdapter Adapter { get; }
        bool IsWeb { get; }
    }
}
