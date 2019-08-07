using NUnit.Framework;
using DevExpress.EasyTest.Framework;

namespace EasyTest.Tests
{
    [TestFixture]
    public class WebTests : CommonTests<WebTestApplicationHelper>
    {
        [Test]
        public void ChangeContactNameTest() => ChangeContactNameTest_();

        [Test]
        public void WorkingWithTasks() => WorkingWithTasks_();

        [Test]
        public void ChangeContactNameAgainTest()
            => ChangeContactNameAgainTest_();

        [Test]
        public void UnlinkActionTest()
        {
            commandAdapter.DoAction("Navigation", "Department");
            commandAdapter.ProcessRecord("Department", new string[] { "Title" }, new string[] { "Development Department" }, "");

            commandAdapter.DoAction("Positions", null);

            ITestControl gridControl = adapter.CreateTestControl(TestControlType.Table, "Positions");
            Assert.AreEqual(2, gridControl.GetInterface<IGridBase>().GetRowCount());

            Assert.AreEqual("Developer", commandAdapter.GetCellValue("Positions", 0, "Title"));

            ITestControl unlink = adapter.CreateTestControl(TestControlType.Action, "Positions.Unlink");
            Assert.IsFalse(unlink.GetInterface<IControlEnabled>().Enabled);


            gridControl.GetInterface<IGridRowsSelection>().SelectRow(0);

            Assert.IsTrue(unlink.GetInterface<IControlEnabled>().Enabled);
            commandAdapter.DoAction("Positions.Unlink", null);

            Assert.AreEqual(1, gridControl.GetInterface<IGridBase>().GetRowCount());
            Assert.AreEqual("Manager", commandAdapter.GetCellValue("Positions", 0, "Title"));

            commandAdapter.DoAction("Contacts", null);
            unlink = adapter.CreateTestControl(TestControlType.Action, "Contacts.Unlink");
            Assert.IsFalse(unlink.GetInterface<IControlEnabled>().Enabled);
        }
    }
}
