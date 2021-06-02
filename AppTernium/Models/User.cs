using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTernium.Models
{
    public class User
    {
        public IList<string> usertype { get; set; }
        public string _id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int streak { get; set; }
        public string scores { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }
}
