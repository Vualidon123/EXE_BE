namespace EXE_BE.Models
{
    public class Notify
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public NotifyReason Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; } // User ID who created the notification
        public bool IsActive { get; set; } = true;
    }

    public enum NotifyReason
    {
        General = 1,
        SystemMaintenance = 2,
        FeatureUpdate = 3,
        SecurityAlert = 4,
        PromotionalOffer = 5,
        PolicyUpdate = 6,
        UserWarning = 7,
        ChallengeAnnouncement = 8,
        EventNotification = 9,
        SystemAlert = 10
    }
}