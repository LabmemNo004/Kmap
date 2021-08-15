using System.Collections.Generic;
using Neo4j.Driver;
using NetCoreBackend.Service;
using NetCoreBackend.Models;
using System.Threading.Tasks;
using Neo4jService;
using ComModule;
namespace NetCoreBackend.Service

{
    public static class SpanService
    {
        public static async void SpanPerson(List<Organization> organizations, List<Officership> officerships,
            string statementText,string id,
            Dictionary<string, object> statementParameters)
        {
            using (var session_async = Neo4JService.Neo4jDriver.AsyncSession())
            {
                //Async Query Neo4j
                var results = new List<Dictionary<string,INode>>();
                try
                {
                    var reader = await session_async.RunAsync(statementText,statementParameters
                    );
                    
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current[0].As<INode>();
                        var link = reader.Current[1].As<INode>();
                        Dictionary<string, INode> record = new Dictionary<string, INode>();
                        record["node"] = node;
                        record["link"] = link;
                        results.Add(record);
                    }
                }
                finally
                {
                    await session_async.CloseAsync();
                }
                
                //Data Preprocess and DAO
                Dictionary<string,List<string>> orgs = new Dictionary<string,List<string>>();
                foreach (var record in results)
                {
                    var node_organization=record["node"].As<INode>();
                    var node_officership=record["link"].As<INode>();
                    string id_organization = node_organization.Id.As<string>();
                    string officership=node_officership.Properties["ns7__hasReportedTitle"].As<string>();

                    if (orgs.ContainsKey(id_organization))
                    {
                        orgs[id_organization].Add(officership);
                        continue;
                    }
                    List<string> off = new List<string>();
                    off.Add(officership);
                    orgs.Add(id_organization,off);
                    
                    var properties = node_organization.Properties;
                    string url = "unknown";
                    string score = "unknown";
                    string organizationName = "unknown";
                    string headQuartersAddress = "unknown";
                    string registeredAddress = "unknown";
                    string headQuartersPhoneNumber = "unknown";
                    string organizationFoundedDate = "unknown";
                    try
                    {
                        url = properties["uri"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        score = properties["ns3__score"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        organizationName = properties["ns4__organization-name"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        headQuartersAddress = properties["ns2__HeadquartersAddress"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        registeredAddress = properties["ns2__RegisteredAddress"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        headQuartersPhoneNumber = properties["ns3__hasHeadquartersPhoneNumber"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        organizationFoundedDate = properties["ns3__hasLatestOrganizationFoundedDate"].As<string>();
                    }
                    catch
                    {
                    }

                    organizations.Add(new Organization
                    {
                        Id_neo4j = id_organization,
                        Url = url,
                        Score = score,
                        OrganizationName = organizationName,
                        HeadQuartersAddress = headQuartersAddress,
                        RegisteredAddress = registeredAddress,
                        HeadQuartersPhoneNumber = headQuartersPhoneNumber,
                        OrganizationFoundedDate = organizationFoundedDate
                    });
                }
                
                Dictionary<string, List<string>>.KeyCollection keyCol = orgs.Keys;

                foreach (string s in keyCol)
                {
                    //Using Com Module
                    List<string> officerRole = orgs[s];
                    Tools tools = new Tools();
                    officerRole=tools.CheckDuplicate(officerRole);
                    officerships.Add(new Officership
                    {
                        PersonId = id,
                        OrganizationId = s,
                        OfficerRole = officerRole
                    });               
                }
            }
        }
        
        
        
        public static async void SpanOrganization(List<Person> people, List<Officership> officerships,
            string statementText,string id,
            Dictionary<string, object> statementParameters)
        {
            using (var session_async = Neo4JService.Neo4jDriver.AsyncSession())
            {
                //Async Query Neo4j
                var results = new List<Dictionary<string,INode>>();
                try
                {
                    var reader = await session_async.RunAsync(statementText,statementParameters
                    );
                    
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current[0].As<INode>();
                        var link = reader.Current[1].As<INode>();
                        Dictionary<string, INode> record = new Dictionary<string, INode>();
                        record["node"] = node;
                        record["link"] = link;
                        results.Add(record);
                    }
                }
                finally
                {
                    await session_async.CloseAsync();
                }
                
                //Data Preprocess and DAO
                Dictionary<string,List<string>> orgs = new Dictionary<string,List<string>>();
                foreach (var record in results)
                {
                    var node_person=record["node"];
                    var node_officership=record["link"];
                    string id_person = node_person.Id.As<string>();
                    string officership=node_officership.Properties["ns7__hasReportedTitle"].As<string>();

                    if (orgs.ContainsKey(id_person))
                    {
                        orgs[id_person].Add(officership);
                        continue;
                    }
                    List<string> off = new List<string>();
                    off.Add(officership);
                    orgs.Add(id_person,off);
                    
                    var properties = node_person.Properties;
                    string url = "unknown";
                    string score = "unknown";
                    string givenName = "unknown";
                    string familyName = "unknown";
                    string honorific = "unknown";
                    try
                    {
                        url = properties["uri"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        score = properties["ns7__score"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        givenName = properties["ns4__given-name"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        familyName = properties["ns4__family-name"].As<string>();
                    }
                    catch
                    {
                    }

                    try
                    {
                        honorific = properties["ns4__honorific-prefix"].As<string>();
                    }
                    catch
                    {
                    }

                    people.Add(new Person
                    {
                        Id_neo4j = id_person,
                        Url = url,
                        Score = score,
                        GivenName = givenName,
                        FamilyName = familyName,
                        Honorific = honorific
                    });
                }
                
                Dictionary<string, List<string>>.KeyCollection keyCol = orgs.Keys;

                foreach (string s in keyCol)
                {
                    //Using Com Module
                    List<string> officerRole = orgs[s];
                    Tools tools = new Tools();
                    officerRole=tools.CheckDuplicate(officerRole);
                    officerships.Add(new Officership
                    {
                        PersonId = s,
                        OrganizationId = id,
                        OfficerRole = officerRole
                    });               
                }
            }
        }
    }
}