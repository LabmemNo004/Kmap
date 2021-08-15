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
    [Route("span/person")]
    public class SpanPersonController : Controller
    {
        private readonly ILogger<SpanPersonController> _logger;

        public SpanPersonController(ILogger<SpanPersonController> logger)
        {
            _logger = logger;
        }
        
        //span person
        [EnableCors]
        [HttpGet]
        public IActionResult Get(string id)
        {
            var organizations = new List<Organization>();
            var officerships = new List<Officership>();
            var statementText = "match (n:ns7__Person)-[r1]-(link:ns7__Officership)-[r2]-(o:ns3__Organization)where id(n)=$id return distinct o,link";
            var statementParameters = new Dictionary<string, object> {{"id", id.As<int>()}};
            SpanService.SpanPerson(organizations,officerships,statementText,id,statementParameters);
            return Ok(new {organizations,officerships});
        }
    }
    
    
    [ApiController]
    [Route("span/organization")]
    public class SpanOrganizationController : Controller
    {
        private readonly ILogger<SpanOrganizationController> _logger;

        public SpanOrganizationController(ILogger<SpanOrganizationController> logger)
        {
            _logger = logger;
        }
        
        //span organization
        [EnableCors]
        [HttpGet]
        public IActionResult Get(string id)
        {
            var people = new List<Person>();
            var officerships = new List<Officership>();
            var statementText = "match (n:ns7__Person)-[r1]-(link:ns7__Officership)-[r2]-(o:ns3__Organization)where id(o)=$id return distinct n,link";
            var statementParameters = new Dictionary<string, object> {{"id", id.As<int>()}};
            SpanService.SpanOrganization(people,officerships,statementText,id,statementParameters);
            return Ok(new {people,officerships});
        }
    }
}