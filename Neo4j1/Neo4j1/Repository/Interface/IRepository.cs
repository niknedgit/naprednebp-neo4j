using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.Repository.Interface
{
    public interface IRepository<Type>
    {
            //Type Get(int identifier);
           // Type Get(string identifier);
            
            Type Create(Type typeInstance);
            void Delete(string identifier);
            Type Update(Type typeInstance);
            List<Type> GetAll();
    }
}
