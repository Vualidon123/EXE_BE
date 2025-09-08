using EXE_BE.Models.ItemList;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXE_BE.Models
{
    public class PlasticUsage
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public  DateTime date {  get; set; }
        public float CO2emission { get; set; }

        public virtual UserActivities? UserActivities { get; set; }
        public List<PlasticItem>? PlasticItems { get; set;}
    }
}
