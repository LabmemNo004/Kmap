namespace NetCoreBackend.Models
{
    public class Organization
    {
        public string Label { get; set; } = "Organization";
        
        public string Id_neo4j { get; set; }
        public string Url { get; set; }
        
        public string Score { get; set; }
        
        public string OrganizationName { get; set; }

        public string HeadQuartersAddress { get; set; }
        
        public string RegisteredAddress { get; set; }
        
        public string HeadQuartersPhoneNumber { get; set; }
        
        public string OrganizationFoundedDate { get; set; }
        
    }
}