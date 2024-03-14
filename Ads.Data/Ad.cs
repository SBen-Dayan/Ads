using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } 
        public DateTime Date { get; set; }
        public string PhoneNumber { get; set; }
        public string Details { get; set; }
    }
}
