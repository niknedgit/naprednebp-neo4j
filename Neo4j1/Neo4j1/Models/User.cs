using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<User> Friends { get; set; }
        public List<Food> TastedFood { get; set; }

        public User()
        {
            Friends = new List<User>();
            TastedFood = new List<Food>();
        }
    }
}