using EXE_BE.Controllers.ViewModel;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using EXE_BE.Services.Models;

namespace EXE_BE.Services
{
    public class NotifyService
    {
        private readonly NotifyRepository _notifyRepository;

        public NotifyService(NotifyRepository notifyRepository)
        {
            _notifyRepository = notifyRepository;
        }

        public async Task<ServiceResponse<NotifyResponseDto>> CreateNotificationAsync(NotifyRequest request, int createdBy)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Content))
            {
                return ServiceResponse<NotifyResponseDto>.FailureResponse("Title and content are required");
            }

            var notify = new Notify
            {
                Title = request.Title,
                Content = request.Content,
                Reason = request.Reason,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdNotify = await _notifyRepository.CreateAsync(notify);
            var responseDto = MapToResponseDto(createdNotify);

            return ServiceResponse<NotifyResponseDto>.SuccessResponse(responseDto, "Notification created successfully");
        }

        public async Task<ServiceResponse<NotifyResponseDto>> GetNotificationByIdAsync(int id)
        {
            var notify = await _notifyRepository.GetByIdAsync(id);
            if (notify == null)
            {
                return ServiceResponse<NotifyResponseDto>.FailureResponse("Notification not found");
            }

            var responseDto = MapToResponseDto(notify);
            return ServiceResponse<NotifyResponseDto>.SuccessResponse(responseDto);
        }

        public async Task<ServiceResponse<List<NotifyResponseDto>>> GetAllNotificationsAsync()
        {
            var notifications = await _notifyRepository.GetAllAsync();
            var responseDtos = notifications.Select(MapToResponseDto).ToList();

            return ServiceResponse<List<NotifyResponseDto>>.SuccessResponse(responseDtos);
        }

        public async Task<ServiceResponse<List<NotifyResponseDto>>> GetActiveNotificationsAsync()
        {
            var notifications = await _notifyRepository.GetActiveNotificationsAsync();
            var responseDtos = notifications.Select(MapToResponseDto).ToList();

            return ServiceResponse<List<NotifyResponseDto>>.SuccessResponse(responseDtos);
        }

        public async Task<ServiceResponse<List<NotifyResponseDto>>> GetNotificationsByReasonAsync(NotifyReason reason)
        {
            var notifications = await _notifyRepository.GetByReasonAsync(reason);
            var responseDtos = notifications.Select(MapToResponseDto).ToList();

            return ServiceResponse<List<NotifyResponseDto>>.SuccessResponse(responseDtos);
        }

        public async Task<ServiceResponse<NotifyPagedResponse>> GetPagedNotificationsAsync(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return ServiceResponse<NotifyPagedResponse>.FailureResponse("Page and pageSize must be greater than 0");
            }

            var notifications = await _notifyRepository.GetPagedAsync(page, pageSize);
            var totalCount = await _notifyRepository.GetTotalCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var responseDtos = notifications.Select(MapToResponseDto).ToList();

            var pagedResponse = new NotifyPagedResponse
            {
                Notifications = responseDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return ServiceResponse<NotifyPagedResponse>.SuccessResponse(pagedResponse);
        }

        public async Task<ServiceResponse<NotifyResponseDto>> UpdateNotificationAsync(int id, NotifyUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Content))
            {
                return ServiceResponse<NotifyResponseDto>.FailureResponse("Title and content are required");
            }

            var existingNotify = await _notifyRepository.GetByIdAsync(id);
            if (existingNotify == null)
            {
                return ServiceResponse<NotifyResponseDto>.FailureResponse("Notification not found");
            }

            var updatedNotify = new Notify
            {
                Id = id,
                Title = request.Title,
                Content = request.Content,
                Reason = request.Reason,
                IsActive = request.IsActive
            };

            var result = await _notifyRepository.UpdateAsync(updatedNotify);
            if (result == null)
            {
                return ServiceResponse<NotifyResponseDto>.FailureResponse("Failed to update notification");
            }

            var responseDto = MapToResponseDto(result);
            return ServiceResponse<NotifyResponseDto>.SuccessResponse(responseDto, "Notification updated successfully");
        }

        public async Task<ServiceResponse<bool>> DeleteNotificationAsync(int id)
        {
            var success = await _notifyRepository.DeleteAsync(id);
            if (!success)
            {
                return ServiceResponse<bool>.FailureResponse("Notification not found or could not be deleted");
            }

            return ServiceResponse<bool>.SuccessResponse(true, "Notification deleted successfully");
        }

        public async Task<ServiceResponse<bool>> SoftDeleteNotificationAsync(int id)
        {
            var success = await _notifyRepository.SoftDeleteAsync(id);
            if (!success)
            {
                return ServiceResponse<bool>.FailureResponse("Notification not found or could not be deactivated");
            }

            return ServiceResponse<bool>.SuccessResponse(true, "Notification deactivated successfully");
        }

        private NotifyResponseDto MapToResponseDto(Notify notify)
        {
            return new NotifyResponseDto
            {
                Id = notify.Id,
                Title = notify.Title,
                Content = notify.Content,
                Reason = notify.Reason,
                ReasonText = GetReasonText(notify.Reason),
                CreatedAt = notify.CreatedAt,
                UpdatedAt = notify.UpdatedAt,
                CreatedBy = notify.CreatedBy,
                IsActive = notify.IsActive
            };
        }

        private string GetReasonText(NotifyReason reason)
        {
            return reason switch
            {
                NotifyReason.General => "General",
                NotifyReason.SystemMaintenance => "System Maintenance",
                NotifyReason.FeatureUpdate => "Feature Update",
                NotifyReason.SecurityAlert => "Security Alert",
                NotifyReason.PromotionalOffer => "Promotional Offer",
                NotifyReason.PolicyUpdate => "Policy Update",
                NotifyReason.UserWarning => "User Warning",
                NotifyReason.ChallengeAnnouncement => "Challenge Announcement",
                NotifyReason.EventNotification => "Event Notification",
                NotifyReason.SystemAlert => "System Alert",
                _ => "Unknown"
            };
        }
    }
}