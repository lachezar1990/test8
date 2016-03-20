using ZapaziMi.WebAPI.Services.Models.Enums;

namespace ZapaziMi.WebAPI.Services.Models
{
    /// <summary>
    /// This class is used for the response model.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// the type of the error
        /// </summary>
        public ErrorTypes ErrorType { get; set; }
        /// <summary>
        /// The short message is used for a display message. Usually this is the error which the user sees when something goes wrong.
        /// </summary>
        public string ShortMessage { get; set; }
        /// <summary>
        /// The long message is used usually for debugging.
        /// </summary>
        public string LongMessage { get; set; }
    }
}
