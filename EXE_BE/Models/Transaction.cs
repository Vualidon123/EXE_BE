namespace EXE_BE.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TransactionStatus Status { get; set; }
        public long Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; }
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Cancelled = 3
    }
}
