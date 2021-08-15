namespace NetCoreBackend.Models
{
    public class Person
        {
            public string Label { get; set; } = "Person";
            
            public string Id_neo4j { get; set; }
            
            public string Url { get; set; }
            
            public string GivenName { get; set; }
            
            public string FamilyName { get; set; }
            
            public string Honorific { get; set; }
            
            public string Score { get; set; }
    
        }
}