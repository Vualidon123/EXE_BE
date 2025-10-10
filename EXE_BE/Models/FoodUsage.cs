using EXE_BE.Models.ItemList;
using System.Text.Json.Serialization;

namespace EXE_BE.Models
{
    public class FoodUsage
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime date { get; set; }
        public float CO2emission { get; set; }
        /*public Food_category foodCategory { get; set; }*/
        public int score { get; set; }
        public List<FoodItem>? FoodItems { get; set; }
        
        public virtual UserActivities? UserActivities { get; set; }
    }
   
}
