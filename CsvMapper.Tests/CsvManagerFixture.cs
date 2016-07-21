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
        protected readonly string FileName = "USPresident_Wikipedia.csv";
        protected readonly string RootFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
        private string _fullFilePath;

        [SetUp]
        public void Setup()
        {
            _fullFilePath = Path.Combine(RootFolder, FileName);
        }

        [Test]
        public void Load_Us_Presidents_Test()
        {
            var presidentsManager = new CsvManager<UsPresident>(_fullFilePath, true);
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
        public void Load_Property_Test()
        {
            CsvManager<Property> propertyManager = new CsvManager<Property>(@"D:\tempSplitFiles\NM_HOMECHECKER_FINAL_000030.txt", true, '|');
            propertyManager.SetField(x => x.OSAPR, 0);
            propertyManager.SetField(x => x.TOID, 1);
            propertyManager.SetField(x => x.PC_AREA, 2);
            propertyManager.SetField(x => x.PC_DISTRICT, 3);
            propertyManager.SetField(x => x.PC_SECTOR, 4);
            propertyManager.SetField(x => x.POSTCODE, 5);
            propertyManager.SetField(x => x.PC_NOSP, 6);
            propertyManager.SetField(x => x.DPS, 7);
            propertyManager.SetField(x => x.ADDRESS_16CHAR, 8);
            propertyManager.SetField(x => x.EASTING, 9);
            propertyManager.SetField(x => x.NORTHING, 10);
            propertyManager.SetField(x => x.DEPARTMENT_NAME, 11);
            propertyManager.SetField(x => x.PO_BOX, 12);
            propertyManager.SetField(x => x.ORGANISATION_NAME, 13);
            propertyManager.SetField(x => x.BUILDING_NUMBER, 14);
            propertyManager.SetField(x => x.SUB_BUILDING_NAME, 15);
            propertyManager.SetField(x => x.BUILDING_NAME, 16);
            propertyManager.SetField(x => x.THOROUGHFARE, 17);
            propertyManager.SetField(x => x.DEPENDENT_THOROUGHFARE, 18);
            propertyManager.SetField(x => x.POST_TOWN, 19);
            propertyManager.SetField(x => x.DEPENDENT_LOCALITY, 20);

            propertyManager.SetField(x => x.DOUBLE_DEPENDENT_LOCALITY, 21);
            propertyManager.SetField(x => x.STATS_FL_VERD_14, 22);
            propertyManager.SetField(x => x.BUILD_DIST_F_FL_14_A, 23);
            propertyManager.SetField(x => x.STORM_RISK, 24);
            propertyManager.SetField(x => x.SUB_RISK, 25);
            propertyManager.SetField(x => x.THEFT_RISK, 26);
            propertyManager.SetField(x => x.SSWELL_OF_RISK, 27);
            propertyManager.SetField(x => x.SLOPE_OF_RISK, 28);
            propertyManager.SetField(x => x.RSAND_OF_RISK, 29);
            propertyManager.SetField(x => x.DISSOL_OF_RISK, 30);


            propertyManager.SetField(x => x.COMP_OF_RISK, 31);
            propertyManager.SetField(x => x.COLLAPS_OF_RISK, 32);
            propertyManager.SetField(x => x.OTHER_PROPERTY_FACTORS, 33);
            propertyManager.SetField(x => x.NEIGHBOURHOOD_RISK, 34);
            propertyManager.SetField(x => x.SMINDEX_100, 35);
            propertyManager.SetField(x => x.UNEMIND_100, 36);
            propertyManager.SetField(x => x.HHDENSIT_100, 37);
            propertyManager.SetField(x => x.DIST_TO_COAST_MILES, 38);
            propertyManager.SetField(x => x.DIST_TO_RIVER_MILES, 39);
            propertyManager.SetField(x => x.AP_HEIGHT_ABOVE_RIVER, 40);

            propertyManager.SetField(x => x.DISTANCE_TO_RAIL_MILES, 41);
            propertyManager.SetField(x => x.DISTANCE_TO_BUS_MILES, 42);
            propertyManager.SetField(x => x.DISTANCE_TO_AIRPORT_MILES, 43);
            propertyManager.SetField(x => x.AIRPORT_CODE, 44);
            propertyManager.SetField(x => x.DIST_TO_UNDERGRD_STATION_MILES, 45);
            propertyManager.SetField(x => x.UNDERGRD_STATION, 46);
            propertyManager.SetField(x => x.ASB_CRIME_RISK, 47);
            propertyManager.SetField(x => x.BURGLARY_CRIME_RISK, 48);
            propertyManager.SetField(x => x.OTHER_CRIME_RISK, 49);
            propertyManager.SetField(x => x.ROBBERY_CRIME_RISK, 50);
            propertyManager.SetField(x => x.VEHICLE_CRIME_RISK, 51);

            propertyManager.SetField(x => x.VIOLENT_CRIME_RISK, 52);
            propertyManager.SetField(x => x.TOTAL_CRIME_RISK, 53);
            propertyManager.SetField(x => x.PC_MOSAIC_UK_6_GROUP_DESC, 54);
            propertyManager.SetField(x => x.STATUS_100, 55);
            propertyManager.SetField(x => x.LIFESTAGE_100, 56);
            propertyManager.SetField(x => x.EXPERIENCE_100, 57);
            propertyManager.SetField(x => x.CULTURE_100, 58);
            propertyManager.SetField(x => x.WORK_100, 59);
            propertyManager.SetField(x => x.FAMILY_LIFESTAGE, 60);
            propertyManager.SetField(x => x.EMPLOYMENT_STATUS, 61);

            propertyManager.SetField(x => x.AFFLUENCE, 62);
            propertyManager.SetField(x => x.KS2_NEAREST_DIST_MILES, 63);
            propertyManager.SetField(x => x.KS2_NEAREST_URN, 64);
            propertyManager.SetField(x => x.KS2_NEAREST_NAME, 65);
            propertyManager.SetField(x => x.KS2_NEAREST_TOWN, 66);
            propertyManager.SetField(x => x.KS2_NEAREST_PCODE, 67);
            propertyManager.SetField(x => x.KS2_NEAREST_TOTPUPS, 68);
            propertyManager.SetField(x => x.KS2_NEAREST_TAPS, 69);
            propertyManager.SetField(x => x.KS2_NEAREST_PERC_LVL4, 70);
            propertyManager.SetField(x => x.KS2_NEAREST_PERC_LVL5, 71);

            propertyManager.SetField(x => x.KS2_NEAREST_PREV_OFSTED, 72);
            propertyManager.SetField(x => x.KS2_NEAREST_CURRENT_OFSTED, 73);
            propertyManager.SetField(x => x.KS2_NEAREST_TAPS_W_B, 74);
            propertyManager.SetField(x => x.KS2_NEAREST_TAPS_B_W, 75);
            propertyManager.SetField(x => x.KS4_NEAREST_DIST_MILES, 76);
            propertyManager.SetField(x => x.KS4_NEAREST_SCHOOL_NAME, 77);
            propertyManager.SetField(x => x.KS4_NEAREST_SCHOOL_PCODE, 78);
            propertyManager.SetField(x => x.KS4_FST_URN, 79);
            propertyManager.SetField(x => x.KS4_FST_TOTPUPS, 80);
            propertyManager.SetField(x => x.KS4_FST_PECR_5_A_C_GCSE, 81);

            propertyManager.SetField(x => x.KS4_FST_AVG_NUM_GCSE, 82);
            propertyManager.SetField(x => x.KS4_FST_TTAPSCP, 83);
            propertyManager.SetField(x => x.KS4_FST_TTAPSCP_100_W_B, 84);
            propertyManager.SetField(x => x.KS4_FST_TTAPSCP_100_B_W, 85);
            propertyManager.SetField(x => x.KS4_NEAR_PREV_OFSTED, 86);
            propertyManager.SetField(x => x.KS4_NEAR_CURR_OFSTED, 87);

            List<Property> propertyResult = propertyManager.GetObjectList();

            Assert.IsNotNull(propertyResult);
            Assert.IsTrue(propertyResult.Count > 0);
        }

    }
}
