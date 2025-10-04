using EXE_BE.Models;

namespace EXE_BE.Controllers.ViewModel
{
    public class NotifyRequest
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public NotifyReason Reason { get; set; }
    }

    public class NotifyUpdateRequest
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public NotifyReason Reason { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class NotifyResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public NotifyReason Reason { get; set; }
        public string ReasonText { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class NotifyPagedResponse
    {
        public List<NotifyResponseDto> Notifications { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}