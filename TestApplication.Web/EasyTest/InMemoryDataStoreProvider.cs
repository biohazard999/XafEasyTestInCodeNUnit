using System;
using System.Collections.Generic;
using System.Web;
using DevExpress.Xpo.DB;
using System.Data;
using DevExpress.Xpo.DB.Helpers;

namespace TestApplication.EasyTest {
    public class InMemoryDataStoreProvider : DevExpress.Xpo.DB.InMemoryDataStore {
        new public const string XpoProviderTypeString = "InMemoryDataSet";
        private static DataSet dataSet = new DataSet();
        private static DataSet savedDataSet;
        private static Dictionary<AutoCreateOption, IDataStore> dic = new Dictionary<AutoCreateOption, IDataStore>();

        new public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            IDataStore testDataStoreSerialized;
            if(!dic.TryGetValue(autoCreateOption, out testDataStoreSerialized)) {
                testDataStoreSerialized = new DataSetDataStore(dataSet, autoCreateOption);
                dic[autoCreateOption] = testDataStoreSerialized;
            }
            objectsToDisposeOnDisconnect = new IDisposable[] { };
            return testDataStoreSerialized;
        }
        static InMemoryDataStoreProvider() {
        }
        new public static void Register() {
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));
        }
        public static bool HasData { get { return savedDataSet != null; } }
        public static void Save() {
            if(!HasData) {
                savedDataSet = dataSet.Copy();
            }
        }
        public static void Reload() {
            if(HasData) {
                dataSet = savedDataSet.Copy();
                dic.Clear();
            }
        }
    }
}
