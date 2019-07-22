using System;
using System.Collections.Generic;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests {
    [TestFixture]
    public class WinTests : CommonTests<WinTestApplicationHelper> {
        [Test]
        public void ChangeContactNameTest() {
            ChangeContactNameTest_();
        }
        [Test]
        public void WorkingWithTasks() {
            WorkingWithTasks_();
        }
    }
}
