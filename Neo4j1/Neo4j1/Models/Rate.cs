using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Models
{
    public class Rate
    {
        public string Id { get; set; }
        public float Rating { get; set; }
        public string Username { get; set; }
    }
}
