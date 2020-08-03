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
    public class UserRepository : IRepository<User>
    {
        private GraphClient client;

        public UserRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public User Create(User typeInstance)
        {
            if (UsernameExist(typeInstance.Username))
                return null;

            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Username", typeInstance.Username);
            queryDictionary.Add("Name", typeInstance.Name);
            queryDictionary.Add("Password", typeInstance.Password);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:User {Name:'" + typeInstance.Name + "'" +
                                                            ", Username:'" + typeInstance.Username + "'" +
                                                            ", Password:'" + typeInstance.Password + "'}) return n",
                                                            queryDictionary, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            User user = users.Find(x => x.Username == typeInstance.Username);

            return user;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User {Username:'" + identifier + "'}) OPTIONAL MATCH(user) -[relationship]- () DELETE user, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public List<User> Get(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:User) " +
                                                "and exists(n.Username) and n.Username =~'" + username + "' " +
                                                "return n",
                                                            queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            return users;
        }

        public List<User> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) return user",
                                                            queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            return users;
        }

        public User Update(User typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", typeInstance.Username);
            queryDict.Add("Name", typeInstance.Name);
            queryDict.Add("Password", typeInstance.Password);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:User{Username:'" + typeInstance.Username + "'}) " +
                                                            "set n.Name = '" + typeInstance.Name  + "' " +
                                                            "set n.Password = '" + typeInstance.Password + "' " +
                                                            "return n",
                                                             queryDict, CypherResultMode.Set);

            User user = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).First();

            return user;
        }

        public List<Food> TastedFood(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + username + "'}) -[r:tasted]-> (food:Food) RETURN food",
                                                          queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            return food;
        }

        public bool UsernameExist(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) WHERE user.Username = '" + username + "' return user",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users.Count != 0;
        }
      
        public List<Food> GetRecomendedFood(string usernameS, string usernameD)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", usernameS); //!

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (uS:User{Username:'" + usernameS + "'}), (uD:User{Username:'" + usernameD + "'})-[:tasted]-(f:Food)" +
                                                            " WHERE NOT (uS)-[:tasted]-(f) RETURN DISTINCT f;"
                                                           , queryDict, CypherResultMode.Set);

            List<Food> result = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(query).ToList();

            return result;
        }

        public List<User> SearchUser(string username, string password)
        {
            // nije li ovo za kes
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username);
            queryDict.Add("Password", password);

            //var query = new Neo4jClient.Cypher.CypherQuery("MATCH (food:Food) WHERE food.Name CONTAINS '" + name +
            //                                           "' AND food.Description CONTAINS '" + description + "' return food",
            //                                              queryDict, CypherResultMode.Set);
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User {Username:'" + username + "'" +
                                                ", Password:'" + password + "'}) return user",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            return users;
        }

        public bool UserExist(LogIn logIn)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User)" +
                                                    " WHERE user.Username = '" + logIn.Username + "' " +
                                                    " AND user.Password = '" + logIn.Password + "' " +
                                                    "return user",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users.Count != 0;
        }
    }
}
