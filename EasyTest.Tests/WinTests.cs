using System;
using NUnit.Framework;

namespace EasyTest.Tests
{
    [TestFixture]
    public class WinTests : CommonTests<WinTestApplicationHelper>
    {
        [Test]
        public void ChangeContactNameTest()
        {
            ChangeContactNameTest_();
        }

        [Test]
        public void WorkingWithTasks()
        {
            WorkingWithTasks_();
        }
    }
}
