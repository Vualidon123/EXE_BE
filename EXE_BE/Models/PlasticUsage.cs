using System.ComponentModel.DataAnnotations.Schema;

namespace EXE_BE.Models
{
    public class PlasticUsage
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public  DateTime date {  get; set; }
        public float item_count {  get; set; }
        public int score { get; set; }

        public plastic_category plasticCategory { get; set; }
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
