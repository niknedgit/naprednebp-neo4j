using Neo4j1.Models;
using Neo4j1.Repository.Interface;
using Neo4j1.Configuration;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Repository.Repositories
{
    public class FoodRepository : IRepository<Food>
    {
        private GraphClient client;

        public FoodRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public Food Create(Food typeInstance)
        {
            /*string maxId = getMaxId();
            string nid;
            try
            {
                int mId = Int32.Parse(maxId);
                mId++;
                nid = (mId).ToString();
            }
            catch (Exception exception)
            {
                nid = "0";
            }
            */
           
            String nid = Guid.NewGuid().ToString();

            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Name", typeInstance.Name);
            queryDictionary.Add("Type", typeInstance.Type);
            queryDictionary.Add("Description", typeInstance.Description);

            queryDictionary.Add("AverageRating", typeInstance.AverageRating);
            queryDictionary.Add("NumberOfVotes", typeInstance.NumberOfVotes);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Food { Id:'" + nid + "', Name:'" + typeInstance.Name + "', Type:'" + typeInstance.Type
                                                            + "', Description:'" + typeInstance.Description

                                                            + "', AverageRating:'" + typeInstance.AverageRating
                                                            + "', NumberOfVotes:'" + typeInstance.NumberOfVotes + "'}) return n",
                                                            queryDictionary, CypherResultMode.Set);

            List<Food> Food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            Food food = Food.Find(x => x.Id ==nid);

            return food;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (Food:Food {Id:'" + identifier + "'}) OPTIONAL MATCH(Food) -[relationship]- () DELETE Food, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public Food Get(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Food) and exists(n.Id) and n.Id =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            Food food1 = food.Find(x => x.Id == identifier);

            return food1;
        }
        public List<Food> GetById(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Food) and exists(n.Id) and n.Id =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            return food;
        }

        public List<Food> GetName(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Food) and exists(n.Name) and n.Name =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);


            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            return food;
        }

        public List<Food> GetSame(string name, string description, string type)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);
            queryDict.Add("Description", description);
            queryDict.Add("Type", type);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (food:Food {Name:'" + name + "'" +
                                                ", Description:'" + description + "', Type:'" + type + "'}) RETURN food",
                                                            queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            return food;
        }

        public List<Food> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (food:Food) return food",
                                                            queryDict, CypherResultMode.Set);

            var food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query);

            return food as List<Food>;
        }

        public Food Update(Food typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", typeInstance.Name);
            queryDict.Add("Type", typeInstance.Type);
            queryDict.Add("Description", typeInstance.Description);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Food) and exists(n.Name) and n.Name =~ '" + typeInstance.Name +
                "' set n.Name = '" + typeInstance.Name + "', n.Type = '" + typeInstance.Type + "', " +
                "n.Description = '" + typeInstance.Description + "' return n",
                                                             queryDict, CypherResultMode.Set);

            List<Food> Foods = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            Food Food = Foods.Find(x => x.Id == typeInstance.Id);

            return Food;
        }

        public List<Food> SearchFood(string name, string description)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);
            queryDict.Add("Description", description);

            //var query = new Neo4jClient.Cypher.CypherQuery("MATCH (food:Food) WHERE food.Name CONTAINS '" + name +
            //                                           "' AND food.Description CONTAINS '" + description + "' return food",
            //                                              queryDict, CypherResultMode.Set);
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (food:Food {Name:'" + name + "'" +
                                                ", Description:'" + description + "'}) return food",
                                                          queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            return food;
        }

        public List<User> GetTasters(string id)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", id);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) -[r:tasted]-(food:Food {Id:'" + id + "'}) return user",
                                                           queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            return users;
        }
        public List<Picture> GetPictures(string id)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", id);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (picture:Picture) -[r:gallery]-(food:Food {Id:'" + id + "'}) return picture",
                                                           queryDict, CypherResultMode.Set);

            List<Picture> pics = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            return pics;
        }

        public Food RateFood(string id, float rating, string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", id);
            queryDict.Add("rating", rating);
            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food {Id:'" + id + "'}) " +
                "set f.AverageRating = (TOFLOAT(f.AverageRating) * TOFLOAT(f.NumberOfVotes) + TOFLOAT($rating)) / " +
                    "(TOFLOAT(f.NumberOfVotes) + TOFLOAT(1)), " +
                "f.NumberOfVotes = (TOFLOAT(f.NumberOfVotes) + TOFLOAT(1)) RETURN f", queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            Food f = food.Find(x => x.Id == id);

            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + username + "'}) RETURN u", 
                        queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query2).ToList();
            User user = users.Find(x => x.Username == username);

            if (!RelationshipTastedExist(user.Username, f))
                f.Visitors.Add(LinkFromUserToFood(user.Username, f));

            return f;
        }

        public bool RelationshipTastedExist(string username, Food food)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", food.Id);
            queryDict.Add("Username", username);
            
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food {Id:'" + food.Id + "'}), " +
                                                                 "(u:User {Username:'" + username + "'})" +
                                                            "RETURN EXISTS( (u)-[:tasted]->(f) )",
                                                         queryDict, CypherResultMode.Set);
            List<bool> exist = ((IRawGraphClient)client).ExecuteGetCypherResults<bool>(query).ToList();
            
            return exist[0];
        }

        public User LinkFromUserToFood(string username, Food food)
        {
            string name = food.Name;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", food.Id);
            queryDict.Add("Username", username);
            queryDict.Add("Name", name);
             
            var newQuery = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food {Id:'" + food.Id + "'}), " +
                                                                    "(u:User {Username:'" + username + "'}) " +
                                                                    "CREATE(u)-[:tasted]->(f) return u",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(newQuery).ToList();

            User u = users.Find(x => x.Username == username);
            u.TastedFood.Add(food);
            return u;
        }

        public List<Food> GetSimilarlyFood(Food food)
        {
            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Description", food.Description);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (f:Food) " +
                                                           "WHERE f.Description CONTAINS '" + food.Description + "'" +
                                                                "OR '" + food.Description + "' CONTAINS f.Description " +
                                                           "RETURN f",
                                                          queryDictionary, CypherResultMode.Set);

            List<Food> f = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();
            return f;
        }

        public String getMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where exists(n.Id) return max(n.Id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }
    }
}
