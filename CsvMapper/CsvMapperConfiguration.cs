using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvMapper
{
    public class CsvMapperConfiguration
    {
        public bool AutoSet { get; set; }
        public bool FirstLineHeader { get; set; }
        public char Separator { get; set; }

        public CsvMapperConfiguration()
        {
            AutoSet = false;
            FirstLineHeader = false;
            Separator = ',';
        }
    }
}
