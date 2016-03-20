namespace ZapaziMi.WebAPI.Services.Models.Enums
{
    /// <summary>
    /// These are the statuses for the response model.
    /// </summary>
    public enum ResponseStatuses
    {
        Success = 0,
        /// <summary>
        /// Success but there are some warnings.
        /// </summary>
        PartialSuccess = 1,
        Failure = 2
    }
}
