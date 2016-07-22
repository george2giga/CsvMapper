using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CsvMapper.Tests
{
    [TestFixture]
    class CsvManagerFixture
    {
        protected readonly string PresidentsFileName = "USPresident_Wikipedia.csv";
        protected readonly string CustomersFileName = "Customer_data.csv";
        protected readonly string RootFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
        private string _presidentsfullFilePath;
        private string _customersfullFilePath;

        [SetUp]
        public void Setup()
        {
            _presidentsfullFilePath = Path.Combine(RootFolder, PresidentsFileName);
            _customersfullFilePath = Path.Combine(RootFolder, CustomersFileName);
        }

        [Test]
        public void Load_Us_Presidents_Test()
        {
            var presidentsManager = new CsvManager<UsPresident>(_presidentsfullFilePath);
            presidentsManager.SetField(x => x.PresidencyId, 0);
            presidentsManager.SetField(x => x.President, 1);
            presidentsManager.SetField(x => x.WikipediaEntry, 2);
            presidentsManager.SetField(x => x.TookOffice, 3);
            presidentsManager.SetField(x => x.LeftOffice, 4);
            presidentsManager.SetField(x => x.Party, 5);
            presidentsManager.SetField(x => x.Portrait, 6);
            presidentsManager.SetField(x => x.Thumbnail, 7);
            presidentsManager.SetField(x => x.HomeState, 8);
            var presidentsResult = presidentsManager.GetObjectList();

            Assert.IsNotNull(presidentsResult);
            Assert.IsTrue(presidentsResult.Count > 0);
        }

        [Test]
        public void Load_Customer_Test_No_Autoset()
        {
            var customersManager = new CsvManager<Customer>(_customersfullFilePath);

            var customersResult = customersManager.GetObjectList();

            Assert.IsNotNull(customersResult);
            Assert.IsTrue(customersResult.Count > 0);
        }
    }
}
