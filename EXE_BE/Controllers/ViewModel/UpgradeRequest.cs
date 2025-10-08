using System.Text.Json.Serialization;

namespace EXE_BE.Controllers.ViewModel
{
    public class UpgradeRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public UpgradePlan Plan { get; set; }
        public string returnUrl { get; set; } = "http://example.com";
        public string cancelUrl { get; set; } = "";
    }

    public enum UpgradePlan
    {
        Vip_25 = 1,
        Vip_50 = 2
    }
}
