using System.Collections.Generic;

namespace NetCoreBackend.Models
{
    public class Officership
    {
        public string PersonId { get; set; }
        
        public string OrganizationId { get; set; }
        
        public List<string> OfficerRole { get; set; }
    }
}