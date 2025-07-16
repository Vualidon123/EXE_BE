namespace EXE_BE.Models
{
    public class EnergyUsage
    {
       
            public int Id { get; set; }
            public int userId { get; set; }
            public DateTime date { get; set; }
            public float electricityconsumption { get; set; }           
            public int score { get; set; }
        
    }
    
}
