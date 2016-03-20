namespace ZapaziMi.WebAPI.Services.Models.Enums
{
    /// <summary>
    /// Error types for the response model. The numbers of the error are the same as the error types from HTTP request
    /// </summary>
    public enum ErrorTypes
    {
        None = 0,
        Exception = 500,
        /// <summary>
        /// Bad Request
        /// </summary>
        Validation = 400,
        NotFound = 404,
        Unauthorized = 401
    }
}
