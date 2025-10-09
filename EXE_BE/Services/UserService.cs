using EXE_BE.Controllers.ViewModel;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using EXE_BE.Services.Models;
using Google.Apis.Auth;
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
        public async Task<ServiceResponse<Auth>> GoogleLoginAsync(GoogleRequest googleAuthDto)
        {
            try
            {
                // Verify Google ID token
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    googleAuthDto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["GoogleAuth:ClientId"] } // Assuming IConfiguration is injected
                    });

                // Find or create user
                var user = await _userRepository.FindOrCreateUserFromGoogleAsync(payload);

                // Generate JWT token
                var token = GenerateJwtToken(user);
              
               var auth= new Auth
                {
                    Token = token,
                    User = user
                };
                return ServiceResponse<Auth>.SuccessResponse(auth, "Login with Google successful");
            }
            catch (Exception)
            {
                throw new Exception("Invalid Google token");
            }
        }
        public async Task<ServiceResponse<Auth>> RegisterAsync(string username, string email, string password, string? phoneNumber, DateOnly DateOfBirth)
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
                DateOfBirth = DateOfBirth,
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

        public async Task<ServiceResponse<UserResponseDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("User not found");
            }

            var userDto = UserResponseDto.FromUser(user);
            return ServiceResponse<UserResponseDto>.SuccessResponse(userDto, "User retrieved successfully");
        }

        public async Task<ServiceResponse<List<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(UserResponseDto.FromUser).ToList();
            return ServiceResponse<List<UserResponseDto>>.SuccessResponse(userDtos, "Users retrieved successfully");
        }

        public async Task<ServiceResponse<PagedResponse<UserResponseDto>>> GetUsersPagedAsync(int page = 1, int pageSize = 10)
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var users = await _userRepository.GetUsersPagedAsync(page, pageSize);
            var totalCount = await _userRepository.GetUserCountAsync();
            
            var userDtos = users.Select(UserResponseDto.FromUser).ToList();
            var pagedResponse = new PagedResponse<UserResponseDto>(userDtos, page, pageSize, totalCount);
            
            return ServiceResponse<PagedResponse<UserResponseDto>>.SuccessResponse(pagedResponse, "Users retrieved successfully");
        }

        public async Task<ServiceResponse<UserResponseDto>> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("User not found");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.UserName))
            {
                // Check if new username is already taken (excluding current user)
                var existingUser = await _userRepository.GetByUsernameAsync(request.UserName);
                if (existingUser != null && existingUser.Id != id)
                {
                    return ServiceResponse<UserResponseDto>.FailureResponse("Username already in use");
                }
                user.UserName = request.UserName;
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            if (request.DateOfBirth.HasValue)
            {
                user.DateOfBirth = request.DateOfBirth.Value;
            }

            await _userRepository.UpdateUserAsync(user);
            var userDto = UserResponseDto.FromUser(user);
            return ServiceResponse<UserResponseDto>.SuccessResponse(userDto, "User updated successfully");
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
        {
            if (!await _userRepository.UserExistsAsync(id))
            {
                return ServiceResponse<bool>.FailureResponse("User not found");
            }

            var result = await _userRepository.DeleteUserAsync(id);
            if (result)
            {
                return ServiceResponse<bool>.SuccessResponse(true, "User deleted successfully");
            }

            return ServiceResponse<bool>.FailureResponse("Failed to delete user");
        }

        public async Task<ServiceResponse<UserResponseDto>> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("User not found");
            }

            // Verify current password
            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("Current password is incorrect");
            }

            // Validate new password
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("New password must be at least 6 characters long");
            }

            // Update password
            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user);

            var userDto = UserResponseDto.FromUser(user);
            return ServiceResponse<UserResponseDto>.SuccessResponse(userDto, "Password changed successfully");
        }

        public async Task<ServiceResponse<UserResponseDto>> UpdateUserRoleAsync(int id, user_role newRole)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.FailureResponse("User not found");
            }

            user.Role = newRole;
            await _userRepository.UpdateUserAsync(user);

            var userDto = UserResponseDto.FromUser(user);
            return ServiceResponse<UserResponseDto>.SuccessResponse(userDto, "User role updated successfully");
        }
    }
}