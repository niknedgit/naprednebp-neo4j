using Neo4j1.DataLayerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Utilities
{
    public static class ObjectFactory
    {
        public static DataLayerService.DataLayerService GetDataLayerService()
        {
            return new DataLayerService.DataLayerService();
        }
    }
}
