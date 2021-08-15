using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using NetCoreBackend.Service;
using NetCoreBackend.Models;
using Org.BouncyCastle.Security;

namespace NetCoreBackend.Controllers
{
    [ApiController]
    [Route("search/person")]
    public class SearchPersonController : Controller
    {
        private readonly ILogger<SearchPersonController> _logger;

        public SearchPersonController(ILogger<SearchPersonController> logger)
        {
            _logger = logger;
        }
        

        //search person
        [EnableCors]
        [HttpGet]
        public IActionResult Get(string name = "Jack", int limit = 5)
        {
            var people = new List<Person>();
            var statementText = "MATCH (n:ns7__Person)where n.`ns4__given-name`contains $name return n limit $limit";
            var statementParameters = new Dictionary<string, object> {{"name", name}, {"limit", limit}};
            SearchService.SearchPerson(people,statementText,statementParameters);
            return Ok(new {people});
        }
    }
    
    [ApiController]
    [Route("search/organization")]
    public class SearchOrganizationController : Controller
    {
        private readonly ILogger<SearchOrganizationController> _logger;

        public SearchOrganizationController(ILogger<SearchOrganizationController> logger)
        {
            _logger = logger;
        }
        
        //search organization
        [EnableCors]
        [HttpGet]
        public IActionResult Get(string name = "Alibaba", int limit = 5)
        {
            var organizations = new List<Organization>();
            var statementText =
                "MATCH (n:ns3__Organization) where n.`ns4__organization-name`contains $name RETURN n LIMIT $limit";
            var statementParameters = new Dictionary<string, object> {{"name", name}, {"limit", limit}};
            SearchService.SearchOrganization(organizations,statementText,statementParameters);
            return Ok(new {organizations});
        }
    }
}