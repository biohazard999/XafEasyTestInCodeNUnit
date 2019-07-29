using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using DevExpress.ExpressApp.EasyTest.WebAdapter.TestControls;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests {

    [TestFixture]
    public class WebTests : CommonTests<WebTestApplicationHelper> {
        [Test, Order(1)]
        public void ChangeContactNameTest() {
            ChangeContactNameTest_();
        }

        [Test, Order(2)]
        public void WorkingWithTasks() {
            WorkingWithTasks_();
        }

        [Test, Order(3)]
        public void ChangeContactNameAgainTest() {
            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.AreEqual("Mary Tellitson", commandAdapter.GetCellValue("Contact", 1, "Full Name"));

            commandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");
            commandAdapter.DoAction("Edit", null);

            Assert.AreEqual("Mary Tellitson", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Development Department", commandAdapter.GetFieldValue("Department"));

            commandAdapter.SetFieldValue("First Name", "User_1");
            commandAdapter.SetFieldValue("Last Name", "User_2");

            commandAdapter.DoAction("Save", null);
            commandAdapter.DoAction("Navigation", "Contact");

            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.AreEqual("User_1 User_2", commandAdapter.GetCellValue("Contact", 1, "Full Name"));

        }

        [Test, Order(4)]
        public void UnlinkActionTest() {
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
