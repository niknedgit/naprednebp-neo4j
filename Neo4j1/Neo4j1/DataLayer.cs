using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j1.Utilities;
using Neo4j1.DataLayerService;

namespace Neo4j1.Controllers
{
    public class DataLayer
    {
        private static DataLayerService.DataLayerService _factory = null;
        private static object objLock = new object();

        public static DataLayerService.DataLayerService GetService()
        {
            if (_factory == null)
            {
                lock (objLock)
                {
                    if (_factory == null)
                        _factory = ObjectFactory.GetDataLayerService();
                }
            }

            return _factory;
        }

       
    }
}
