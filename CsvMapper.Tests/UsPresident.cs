using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvMapper.Tests
{
    public class UsPresident
    {
        public int PresidencyId { get; set; }
        public string President { get; set; }
        public string WikipediaEntry { get; set; }
        public DateTime TookOffice { get; set; }
        public DateTime LeftOffice { get; set; }
        public string Party { get; set; }
        public string Portrait { get; set; }
        public string Thumbnail { get; set; }
        public string HomeState { get; set; }
    }
}
