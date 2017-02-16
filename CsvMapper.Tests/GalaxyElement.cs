using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CsvMapper.Tests
{
    //4% of Sloan Digital Sky galaxy objects	
    public class GalaxyElement
    {
        public long ObjectId { get; set; }
        public float Right_Ascension { get; set; }
        public float Declination { get; set; }
        public double Ultraviolet { get; set; }
        public double Green { get; set; }
        public double Red { get; set; }
        public double Infrared { get; set; }
        public double Z { get; set; }
    }
}
