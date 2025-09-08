namespace EXE_BE.Models.ItemList
{
    public class FoodItem
    {
        public int Id { get; set; }
        public food_category FoodCategory { get; set; } // Category of food item
        public float Weight { get; set; } // Weight in kg
        public int FoodUsageId { get; set; } // Foreign key to FoodUsage
    }
    public enum food_category
    {
        Beef = 1,
        Lamb = 2,
        Pork = 3,
        Chicken = 4,
        Fish = 5,
        Eggs = 6,
        Rice = 7,
        Vegetables = 8,
        Others = 9
    }
}
