using System;
using System.Collections.Generic;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests
{
    public abstract class CommonTests<T> : EasyTestTestsBase<T> where T : IEasyTestFixtureHelper, new()
    {
        protected void ChangeContactNameTest_()
        {
            var control = adapter.CreateTestControl(TestControlType.Table, "");
            var table = control.GetInterface<IGridBase>();
            Assert.AreEqual(2, table.GetRowCount());

            var column = commandAdapter.GetColumn(control, "Full Name");

            Assert.AreEqual("John Nilsen", table.GetCellValue(0, column));
            Assert.AreEqual("Mary Tellitson", table.GetCellValue(1, column));

            commandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");

            Assert.AreEqual("Mary Tellitson", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Development Department", commandAdapter.GetFieldValue("Department"));
            Assert.AreEqual("Manager", commandAdapter.GetFieldValue("Position"));

            if (IsWeb)
            {
                commandAdapter.DoAction("Edit", null);
            }

            commandAdapter.SetFieldValue("First Name", "User_1");
            commandAdapter.SetFieldValue("Last Name", "User_2");

            commandAdapter.SetFieldValue("Position", "Developer");

            commandAdapter.DoAction("Save", null);

            Assert.AreEqual("User_1 User_2", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Developer", commandAdapter.GetFieldValue("Position"));
        }

        protected void WorkingWithTasks_()
        {
            commandAdapter.DoAction("Navigation", "Default.Demo Task");
            commandAdapter.ProcessRecord("Demo Task", new string[] { "Subject" }, new string[] { "Fix breakfast" }, "");

            var control = adapter.CreateTestControl(TestControlType.Table, "Contacts");
            var table = control.GetInterface<IGridBase>();
            Assert.AreEqual(0, table.GetRowCount());

            commandAdapter.DoAction("Contacts.Link", null);
            control = adapter.CreateTestControl(TestControlType.Table, "Contact");
            control.GetInterface<IGridRowsSelection>().SelectRow(0);
            commandAdapter.DoAction("OK", null);

            control = adapter.CreateTestControl(TestControlType.Table, "Contacts");
            table = control.GetInterface<IGridBase>();
            Assert.AreEqual(1, table.GetRowCount());
            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contacts", 0, "Full Name"));
        }

        protected void ChangeContactNameAgainTest_()
        {
            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.AreEqual("Mary Tellitson", commandAdapter.GetCellValue("Contact", 1, "Full Name"));

            commandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");

            if (IsWeb)
            {
                commandAdapter.DoAction("Edit", null);
            }

            Assert.AreEqual("Mary Tellitson", commandAdapter.GetFieldValue("Full Name"));
            Assert.AreEqual("Development Department", commandAdapter.GetFieldValue("Department"));

            commandAdapter.SetFieldValue("First Name", "User_1");
            commandAdapter.SetFieldValue("Last Name", "User_2");

            commandAdapter.DoAction("Save", null);
            commandAdapter.DoAction("Navigation", "Contact");

            Assert.AreEqual("John Nilsen", commandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.AreEqual("User_1 User_2", commandAdapter.GetCellValue("Contact", 1, "Full Name"));

        }
    }
}
