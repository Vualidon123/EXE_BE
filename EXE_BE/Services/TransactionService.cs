using EXE_BE.Controllers.ViewModel;
using EXE_BE.Services.Models;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using System;
using Net.payOS;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;

namespace EXE_BE.Services
{
    public class TransactionService
    {
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransactionService> _logger;
        private readonly TransactionRepository _transactionRepository;
        private readonly UserRepository _userRepository;

        public TransactionService(
            IConfiguration configuration,
            ILogger<TransactionService> logger,
            PayOS payOS,
            TransactionRepository transactionRepository,
            UserRepository userRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _payOS = payOS;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        // CRUD Operations
        public async Task<ServiceResponse<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest request)
        {
            try
            {
                // Validate user exists
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return ServiceResponse<TransactionResponse>.FailureResponse("User not found");
                }

                var transaction = new EXE_BE.Models.Transaction
                {
                    UserId = request.UserId,
                    Amount = request.Amount,
                    Reason = request.Reason,
                    Status = request.Status
                };

                var createdTransaction = await _transactionRepository.AddTransactionAsync(transaction);
                
                // Get the transaction with user details
                var transactionWithUser = await _transactionRepository.GetTransactionByIdAsync(createdTransaction.Id);
                
                var response = MapToTransactionResponse(transactionWithUser!);
                
                return ServiceResponse<TransactionResponse>.SuccessResponse(response, "Transaction created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction");
                return ServiceResponse<TransactionResponse>.FailureResponse("An error occurred while creating the transaction");
            }
        }

        public async Task<ServiceResponse<TransactionResponse>> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return ServiceResponse<TransactionResponse>.FailureResponse("Transaction not found");
                }

                var response = MapToTransactionResponse(transaction);
                return ServiceResponse<TransactionResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction");
                return ServiceResponse<TransactionResponse>.FailureResponse("An error occurred while retrieving the transaction");
            }
        }

        public async Task<ServiceResponse<PagedResponse<TransactionResponse>>> GetAllTransactionsAsync(TransactionFilterRequest filter)
        {
            try
            {
                var (transactions, totalCount) = await _transactionRepository.GetFilteredTransactionsAsync(filter);
                
                var transactionResponses = transactions.Select(MapToTransactionResponse).ToList();
                
                var pagedResponse = new PagedResponse<TransactionResponse>(
                    transactionResponses, 
                    filter.Page, 
                    filter.PageSize, 
                    totalCount);

                return ServiceResponse<PagedResponse<TransactionResponse>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions");
                return ServiceResponse<PagedResponse<TransactionResponse>>.FailureResponse("An error occurred while retrieving transactions");
            }
        }

        public async Task<ServiceResponse<List<TransactionResponse>>> GetTransactionsByUserIdAsync(int userId)
        {
            try
            {
                var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);
                var response = transactions.Select(MapToTransactionResponse).ToList();
                
                return ServiceResponse<List<TransactionResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user transactions");
                return ServiceResponse<List<TransactionResponse>>.FailureResponse("An error occurred while retrieving user transactions");
            }
        }

        public async Task<ServiceResponse<TransactionResponse>> UpdateTransactionAsync(int id, UpdateTransactionRequest request)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return ServiceResponse<TransactionResponse>.FailureResponse("Transaction not found");
                }

                // Update only provided fields
                if (request.Status.HasValue)
                    transaction.Status = request.Status.Value;

                if (!string.IsNullOrEmpty(request.Reason))
                    transaction.Reason = request.Reason;

                await _transactionRepository.UpdateTransactionAsync(transaction);

                var response = MapToTransactionResponse(transaction);
                return ServiceResponse<TransactionResponse>.SuccessResponse(response, "Transaction updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction");
                return ServiceResponse<TransactionResponse>.FailureResponse("An error occurred while updating the transaction");
            }
        }

        public async Task<ServiceResponse<bool>> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return ServiceResponse<bool>.FailureResponse("Transaction not found");
                }

                await _transactionRepository.DeleteTransactionAsync(id);
                return ServiceResponse<bool>.SuccessResponse(true, "Transaction deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction");
                return ServiceResponse<bool>.FailureResponse("An error occurred while deleting the transaction");
            }
        }

        // Helper method to map Transaction to TransactionResponse
        private TransactionResponse MapToTransactionResponse(EXE_BE.Models.Transaction transaction)
        {
            return new TransactionResponse
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                UserName = transaction.User?.UserName ?? "Unknown",
                Status = transaction.Status,
                Amount = transaction.Amount,
                Reason = transaction.Reason,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }

        // PayOS Related Methods
        public async Task<ServiceResponse<CreatePaymentResult>> GeneratePayOSPaymentUrlAsync(PaymentData data)
        {
            try
            {
                var paymentRes = await _payOS.createPaymentLink(data);

                return ServiceResponse<CreatePaymentResult>.SuccessResponse(paymentRes, "Payment URL generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during payment URL generation");
                return ServiceResponse<CreatePaymentResult>.FailureResponse("An error occurred while generating payment URL.");
            }
        }

        public async Task HandlePayOSReturnAsync(WebhookType req)
        {
            try
            {
                //var data = _payOS.verifyPaymentWebhookData(req);
                var data = req.data;
                // 1. Find the transaction by orderCode
                var transaction = await _transactionRepository.GetTransactionByIdAsync((int)data.orderCode);
                if (transaction == null)
                {
                    _logger.LogError("Transaction not found for orderCode: {OrderCode}", data.orderCode);
                    return;
                }

                // 2. Update transaction status
                if (data.code == "00" && data.amount == transaction.Amount)
                {
                    transaction.Status = TransactionStatus.Completed;
                }
                else
                {
                    transaction.Status = TransactionStatus.Failed;
                    transaction.Reason += ". Chuyển tiền thất bại";
                }
                await _transactionRepository.UpdateTransactionAsync(transaction);

                // 3. If payment successful, update user subscription type
                if (transaction.Status == TransactionStatus.Completed)
                {
                    var user = await _userRepository.GetByIdAsync(transaction.UserId);

                    // Determine subscription type from transaction amount
                    if (transaction.Amount == 25000)
                        user.SubscriptionType = subscription_type.Vip_25;
                    else if (transaction.Amount == 50000)
                        user.SubscriptionType = subscription_type.Vip_50;

                    // Save user update
                    await _userRepository.UpdateUserAsync(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling PayOs return");
                return;
            }
        }
    }
}
