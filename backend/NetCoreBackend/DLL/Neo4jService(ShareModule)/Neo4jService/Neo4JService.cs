using Neo4j.Driver;

namespace Neo4jService
{
    public static class Neo4JService
    {
        public static IDriver Neo4jDriver{ get; private set; }
        

        public static void Register()
        {
            //Use an IoC container and register as a Singleton
            var url = "bolt://1.15.37.186:7687";
            var username = "neo4j";
            var password = "27182818";
            var authToken = AuthTokens.None;

            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
                authToken = AuthTokens.Basic(username, password);


            Neo4jDriver = GraphDatabase.Driver(url, authToken);
        }
    }
}