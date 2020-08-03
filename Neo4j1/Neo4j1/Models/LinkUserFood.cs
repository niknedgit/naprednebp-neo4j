using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Models
{
    public class LinkUserFood
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }

        public string RestaurantName { get; set; }
        public string RestaurantCity { get; set; }

        public float AverageRating { get; set; }
        public int NumberOfVotes { get; set; }

    }
}
