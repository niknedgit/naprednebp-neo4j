using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Neo4j1.Configuration
{
   public interface IConfig
    {
        GraphClient GetNeo4JClient();

    }
}
