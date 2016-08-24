using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace FDB.DataAccessLayer
{
    public abstract class DataAccessBase
    {
        protected Database fdbDB;

        public DataAccessBase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();

            fdbDB = factory.Create("ExampleDatabase");
            //fdbDB = DatabaseFactory.CreateDatabase("ExampleDatabase");            
        }
    }
}
