using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Models
{
    public class Food
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public float AverageRating { get; set; }
        public int NumberOfVotes { get; set; }

        public List<Picture> Pictures { get; set; }
        public List<User> Visitors { get; set; }

        public Food()
        {
            Pictures = new List<Picture>();
            Visitors = new List<User>();
        }
    }
}
