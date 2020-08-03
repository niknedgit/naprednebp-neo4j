using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j1.Models;
using Neo4j1.Repository.Interface;
using Neo4j1.Configuration;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Neo4j1.Repository.Repositories
{
    public class RestaurantRepository : IRepository<Restaurant>
    {
        private GraphClient client;

        public RestaurantRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public Restaurant Create(Restaurant typeInstance)
        {
            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Name", typeInstance.Name);
            queryDictionary.Add("City", typeInstance.City);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Restaurant {Name:'" + typeInstance.Name + "', " +
                                                    "City:'" + typeInstance.City + "'}) return n",
                                                            queryDictionary, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            Restaurant Restaurant = Restaurants.Find(x => x.Name == typeInstance.Name);

            return Restaurant;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (Restaurant:Restaurant {Name:'" + identifier + "'}) OPTIONAL MATCH(Restaurant) -[relationship]- () DELETE Restaurant, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public Restaurant Get(string name, string city)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);
            queryDict.Add("City", city);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Restaurant) and exists(n.Name) " +
                                            "and n.Name =~'" + name + "' and n.City =~'" + city + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            Restaurant Restaurant = Restaurants.Find(x => x.Name == name);

            return Restaurant;
        }

        public List<Restaurant> GetByNC(string name, string city)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);
            queryDict.Add("City", city);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Restaurant) and exists(n.Name) " +
                                            "and n.Name =~'" + name + "' and n.City =~'" + city + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();


            return Restaurants;
        }

        public List<Restaurant> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (Restaurant:Restaurant) return Restaurant",
                                                            queryDict, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            return Restaurants;
        }

        public List<Restaurant> GetByCity(string city)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (r:Restaurant {City:'" + city + "'}) return r",
                                                            queryDict, CypherResultMode.Set);

            List<Restaurant> restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            return restaurants;
        }

        public Restaurant Update(Restaurant typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", typeInstance.Name);
            queryDict.Add("Address", typeInstance.City);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Restaurant) " +
                                        "and exists(n.Name) and n.Name =~ '" + typeInstance.Name + "' " +
                                        "set n.Name = '" + typeInstance.Name + "', " +
                                        "n.City = '" + typeInstance.City + "'  return n",  
                                                             queryDict, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            Restaurant Restaurant = Restaurants.Find(x => x.Name == typeInstance.Name);

            return Restaurant;
        }

        public Restaurant AddFoodToMenu(Food food, string restaurantName,string restaurantCity)
        {
            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Id", food.Id);
            queryDictionary.Add("Type", food.Type);
            queryDictionary.Add("Description", food.Description);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food {Id:'" + food.Id + "'}), " +
                                                "(r:Restaurant {Name:'" + restaurantName + "',City:'" + restaurantCity + "'}) " +
                                                "CREATE(r)-[srv:serve]->(f) RETURN r",
                                                             queryDictionary, CypherResultMode.Set);

            List<Restaurant> Restaurants = ((IRawGraphClient)client).ExecuteGetCypherResults<Restaurant>(query).ToList();

            Restaurant Restaurant = Restaurants.Find(x => x.Name == restaurantName);

            return Restaurant;
        }

        public List<Food> GetMenu(string name, string city)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);
            queryDict.Add("City", city);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (r:Restaurant {Name:'" + name + "', City:'" + city + "'}) -[s:serve]-> (f:Food) " +
                                                           "return f",
                                                           queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            return food;
        }

        public bool RelationshipServeExist(Food food, string restaurantName, string restaurantCity)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", food.Id);
            queryDict.Add("Name", restaurantName);
            queryDict.Add("City", restaurantCity);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food {Id:'" + food.Id + "'}), " +
                                               "(r:Restaurant {Name:'" + restaurantName + "', City:'" + restaurantCity + "'})" +
                                                        "RETURN EXISTS( (r)-[:serve]->(f) )",
                                                         queryDict, CypherResultMode.Set);
            List<bool> exist = ((IRawGraphClient)client).ExecuteGetCypherResults<bool>(query).ToList();

            return exist[0];
        }
    }
}
