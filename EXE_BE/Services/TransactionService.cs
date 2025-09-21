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
