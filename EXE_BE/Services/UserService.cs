using EXE_BE.Controllers.ViewModel;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using EXE_BE.Services.Models;
using Microsoft.IdentityModel.Tokens;
using Net.payOS.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EXE_BE.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly TransactionService _transactionService;
        private readonly TransactionRepository _transactionRepository;
        private readonly IConfiguration _configuration;

        public UserService(UserRepository userRepository, IConfiguration configuration, TransactionService transactionService, TransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _transactionService = transactionService;
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<Auth>> RegisterAsync(string username, string email, string password, string? phoneNumber)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ServiceResponse<Auth>.FailureResponse("Username, email, and password are required");
            }

            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(email))
            {
                return ServiceResponse<Auth>.FailureResponse("Email already in use");
            }

            // Check if username already exists
            if (await _userRepository.UsernameExistsAsync(username))
            {
                return ServiceResponse<Auth>.FailureResponse("Username already in use");
            }

            // Create new user
            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = HashPassword(password),
                PhoneNumber = phoneNumber,
                Role = user_role.User,
                SubscriptionType = subscription_type.Free,
            };

            // Save user to database
            await _userRepository.CreateAsync(user);

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Create Auth object
            var auth = new Auth
            {
                User = user,
                Token = token
            };

            return ServiceResponse<Auth>.SuccessResponse(auth, "Registration successful");
        }

        public async Task<ServiceResponse<Auth>> LoginAsync(string email, string password)
        {
            // Get user by email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return ServiceResponse<Auth>.FailureResponse("Invalid email or password");
            }

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash))
            {
                return ServiceResponse<Auth>.FailureResponse("Invalid email or password");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Create Auth object
            var auth = new Auth
            {
                User = user,
                Token = token
            };

            return ServiceResponse<Auth>.SuccessResponse(auth, "Login successful");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "DefaultKeyForDevelopmentPurposesOnly12345678901234";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(user_role), user.Role)),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "exeapi",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<CreatePaymentResult>> UpgradeAccount(UpgradeRequest request)
        {
            // Build PayOS request payload
            var plan = request.Plan switch
            {
                UpgradePlan.Vip_25 => "Gói 25",
                UpgradePlan.Vip_50 => "Gói 50",
                _ => throw new ArgumentException($"Unsupported plan type: {request.Plan}")
            };
            var amount = request.Plan switch
            {
                UpgradePlan.Vip_25 => 25000,
                UpgradePlan.Vip_50 => 50000,
                _ => throw new ArgumentException($"Unsupported plan type: {request.Plan}")
            };

            var transaction = await _transactionRepository.AddTransactionAsync(new EXE_BE.Models.Transaction
            {
                UserId = request.UserId,
                Amount = amount,
                Status = TransactionStatus.Pending,
                Reason = $"Nâng cấp {plan}"
            });

            var paymentData = new PaymentData(
                orderCode: transaction.Id,
                amount: amount,
                description: $"Nâng cấp {plan}",
                items: new List<ItemData>() {
                        new ItemData(plan, 1, amount)
                },
                cancelUrl: request.cancelUrl,
                returnUrl: request.returnUrl
                );

            var res = await _transactionService.GeneratePayOSPaymentUrlAsync(paymentData);

            if (res.Success && res.Data != null)
            {
                return ServiceResponse<CreatePaymentResult>.SuccessResponse(res.Data, "Payment URL generated successfully");
            }
            else
            {
                return ServiceResponse<CreatePaymentResult>.FailureResponse(res.Message ?? "Failed to generate payment URL");
            }
        }
    }
}