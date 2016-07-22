using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvMapper.Tests
{
    public class Customer
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Ip_address { get; set; }
        public string City { get; set; }
        public long? CreditCard { get; set; }
    }
}
