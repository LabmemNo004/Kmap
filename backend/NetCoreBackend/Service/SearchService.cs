using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Neo4j.Driver;
using NetCoreBackend.Models;
using System.Threading.Tasks;
using Neo4jService;

namespace NetCoreBackend.Service
{
    public static class SearchService
    {
        //Using C++ Win32 Module
        [DllImport("libCppModule.dylib", EntryPoint = "judgeOrganization")]
        public static extern bool judgeOrganization(int score);
        
        public static async void SearchPerson(List<Person> people, string statementText,
            Dictionary<string, object> statementParameters)
        {
            using (var session_async =Neo4JService.Neo4jDriver.AsyncSession())
            {
                //Async Query Neo4j
                var result = new List<INode>();
                try
                {
                    var reader = await session_async.RunAsync(statementText,statementParameters
                    );
                    
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current[0].As<INode>();

                        result.Add(node);
                    }
                }
                finally
                {
                    await session_async.CloseAsync();
                }
                
                //Data Preprocess and DAO
                foreach (var node in result)
                {
                    string id = node.Id.As<string>();
                    var labels = node.Labels;
                    string mainLabel = labels[1];
                    var properties = node.Properties;
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
                        Id_neo4j = id,
                        Url = url,
                        Score = score,
                        GivenName = givenName,
                        FamilyName = familyName,
                        Honorific = honorific
                    });
                    //Using C++ Win32 dll Module
                    Console.Write(judgeOrganization(int.Parse(score)));
                }
            }
            
        }


        public static async void SearchOrganization(List<Organization> organizations, string statementText,
            Dictionary<string, object> statementParameters)
        {
            using (var session_async = Neo4JService.Neo4jDriver.AsyncSession())
            {
                var result = new List<INode>();
                // Async Query Neo4j
                try
                {
                    var reader = await session_async.RunAsync(statementText,statementParameters
                    );
                    
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current[0].As<INode>();

                        result.Add(node);
                    }
                }
                finally
                {
                    await session_async.CloseAsync();
                }
                
                //Data Preprocess and DAO
                foreach (var node in result)
                {
                    var labels = node.Labels;
                    string mainLabel = labels[1];
                    string id = node.Id.As<string>();
                    var properties = node.Properties;

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
                        Id_neo4j = id,
                        Url = url,
                        Score = score,
                        OrganizationName = organizationName,
                        HeadQuartersAddress = headQuartersAddress,
                        RegisteredAddress = registeredAddress,
                        HeadQuartersPhoneNumber = headQuartersPhoneNumber,
                        OrganizationFoundedDate = organizationFoundedDate
                    });
                }
            }
            
        }
    }
}