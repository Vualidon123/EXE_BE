using System.ComponentModel.DataAnnotations;
using EXE_BE.Models;

namespace EXE_BE.Controllers.ViewModel
{
    public class CreateTransactionRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public long Amount { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    }

    public class UpdateTransactionRequest
    {
        public TransactionStatus? Status { get; set; }

        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string? Reason { get; set; }
    }

    public class TransactionResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public TransactionStatus Status { get; set; }
        public long Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class TransactionFilterRequest
    {
        public int? UserId { get; set; }
        public TransactionStatus? Status { get; set; }
        public long? MinAmount { get; set; }
        public long? MaxAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}