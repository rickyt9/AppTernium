using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTernium.Models {
    public class Exam {
        public IList<string> images { get; set; }
        public IList<string> access { get; set; }
        public IList<string> presented { get; set; }
        public string _id { get; set; }
        public string examName { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public string attempts { get; set; }
        public DateTime date { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
        public DateTime dueDate { get; set; }
        public string user { get; set; }
    }
}
