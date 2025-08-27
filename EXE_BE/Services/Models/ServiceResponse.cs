namespace EXE_BE.Services.Models
{
    /// <summary>
    /// Generic response class for service operations.
    /// </summary>
    /// <typeparam name="T">The type of data returned in the response.</typeparam>
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Indicates if the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message describing the result of the operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The data returned from the operation (if any).
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        public static ServiceResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failed response with an error message.
        /// </summary>
        public static ServiceResponse<T> FailureResponse(string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}