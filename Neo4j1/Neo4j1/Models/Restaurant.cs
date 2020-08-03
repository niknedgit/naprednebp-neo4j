using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Models
{
    public class Restaurant
    {
        public string Name { get; set; }
        public string City { get; set; }

        public List<Food> Menu { get; set; }

        public Restaurant()
        {
            Menu = new List<Food>();
        }
    }
}
