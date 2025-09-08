namespace EXE_BE.Models.ItemList
{
    public class PlasticItem
    {
        public int Id { get; set; }
        public plastic_category PlasticCategory { get; set; } // Category of plastic item
        public float Weight { get; set; } // Weight in kg
        public int PlasticUsageId { get; set; } // Foreign key to PlasticUsage
                                                
    }
    public enum plastic_category
    {
        PlasticBottle = 1,
        PlasticBag = 2,
        PlasticCup = 3,
        PlasticStraw = 4,
        PlasticContainer = 5,
        Other = 6
    }
}
