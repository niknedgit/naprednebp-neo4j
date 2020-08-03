using Neo4j1.Configuration;
using Neo4j1.Models;
using Neo4j1.Repository.Interface;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Repository.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private GraphClient client;

        public PictureRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public Picture Create(Picture typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", typeInstance.PictureURL);
            queryDict.Add("FoodId", typeInstance.FoodId);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Picture {PictureURL:'" + typeInstance.PictureURL + "', " +
                                                            "FoodId:'" + typeInstance.FoodId + "'}) return n",
                                                           queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture picture = pictures.Find(x => x.PictureURL == typeInstance.PictureURL);

            return picture;
        }
        
        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (picture:Picture {PictureURL:'" + identifier + "'}) OPTIONAL MATCH(picture) -[relationship]- () DELETE picture, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public Picture Get(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Picture) and exists(n.PictureURL) and n.PictureURL =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture picture = pictures.Find(x => x.PictureURL == identifier);

            return picture;
        }

        public List<Picture> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (picture:Picture) return picture",
                                                            queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            return pictures;
        }

        public Picture Update(Picture typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("FoodId", typeInstance.FoodId);
            queryDict.Add("PictureURL", typeInstance.PictureURL);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Picture) and exists(n.PictureURL) " +
                                                    "and n.PictureURL =~ '" + typeInstance.PictureURL + "' " +
                                                    "set n.FoodId = '" + typeInstance.FoodId + "' return n",
                                                             queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture updatedPicture = pictures.Find(x => x.PictureURL == typeInstance.PictureURL);

            return updatedPicture;
        }


        public Food StoreImageUrl(Picture picture)
        {
            this.Create(picture);
            return this.LinkToFood(picture);
        }

        public Food LinkToFood(Picture picture)
        {
            string url = picture.PictureURL;
            string foodId = picture.FoodId;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", url);
            queryDict.Add("Id", foodId);
            var query = "MATCH (f:Food {Id:'" + foodId + "'}), (picture:Picture {PictureURL:'" + url + "'}) CREATE(f)-[:gallery]->(picture) return f";
            var newQuery = new Neo4jClient.Cypher.CypherQuery(query,
                                                          queryDict, CypherResultMode.Set);

            List<Food> food = ((IRawGraphClient)client).ExecuteGetCypherResults<Food>(newQuery).ToList();

            Food f = food.Find(x => x.Id == foodId);
          //  f.Pictures.Add(picture);
            return f;
        }
    }   
}