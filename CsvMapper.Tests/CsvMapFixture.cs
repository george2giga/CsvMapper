using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CsvMapper.Tests
{
    [TestFixture]
    public class CsvMapFixture
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
            var presidentsManager = new CsvMap<UsPresident>(_presidentsfullFilePath)
                .SetField(x => x.PresidencyId, 0)
                .SetField(x => x.President, 1)
                .SetField(x => x.WikipediaEntry, 2)
                .SetField(x => x.TookOffice, 3)
                .SetField(x => x.LeftOffice, 4)
                .SetField(x => x.Party, 5)
                .SetField(x => x.Portrait, 6)
                .SetField(x => x.Thumbnail, 7)
                .SetField(x => x.HomeState, 8);
            var presidentsResult = presidentsManager.Load().ToList();

            Assert.IsNotNull(presidentsResult);
            Assert.IsTrue(presidentsResult.Count() > 0);
        }


        [Test]
        public void Load_Galaxy_Elements_Test()
        {
            var galaxyElementsMap = new CsvMap<GalaxyElement>("c:\\tempdev\\sloangalaxy.csv", true,',', true);

            galaxyElementsMap
                .SetField(x => x.Right_Ascension, 1);
                
            var result = galaxyElementsMap.Load();

            using (StreamWriter file = new StreamWriter(@"C:\tempdev\WriteLines2.txt"))
            {
                foreach (var galaxyElement in result.Take(1000))
                {
                    //Console.WriteLine(galaxyElement.ObjectId);
                    file.WriteLine(galaxyElement.ObjectId);
                }
            }

           
            //Console.Write(result.Count());
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Any());
        }

        [Test]
        public void Load_Galaxy_Elements_Test_Print_First_Ten_Records()
        {
            var galaxyElementsMap = new CsvMap<GalaxyElement>("c:\\tempdev\\sloangalaxy.csv", true, ',', true);

            galaxyElementsMap
                .SetField(x => x.Right_Ascension, 1);

            var result = galaxyElementsMap.Load();

            
            foreach (var galaxyElement in result.Take(1000))
            {
                Console.WriteLine(galaxyElement.ObjectId);
            }


            //Console.Write(result.Count());
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Any());
        }
    }
}
