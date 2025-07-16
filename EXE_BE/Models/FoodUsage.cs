namespace EXE_BE.Models
{
    public class FoodUsage
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public DateTime date { get; set; }
        /*public float { get; set; }*/
        /*public Food_category foodCategory { get; set; }*/
        public int score { get; set; }
    }
    /*public enum Food_category
    {
        Meat = 1,
        Dairy = 2,
        Vegetables = 3,
        Fruits = 4,
        Grains = 5,
        ProcessedFood = 6,
        Other = 7
    }*/
}
