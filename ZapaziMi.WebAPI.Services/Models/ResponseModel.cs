using System.Collections.Generic;
using ZapaziMi.WebAPI.Services.Models.Enums;

namespace ZapaziMi.WebAPI.Services.Models
{
    /// <summary>
    /// This class is used for all responses to the client-side.
    /// </summary>
    public class ResponseModel
    {
        public ResponseModel()
        {
            Messages = new List<string>();
            Warnings = new List<string>();
            Errors = new List<Error>();
        }

        /// <summary>
        /// This field is read-only. It represents the status of the response.
        /// </summary>
        public ResponseStatuses ResponseStatus
        {
            get
            {
                if (Messages.Count > 0 && Warnings.Count == 0 && Errors.Count == 0)
                {
                    return ResponseStatuses.Success;
                }
                else if (Warnings.Count > 0 && Errors.Count == 0)
                {
                    return ResponseStatuses.PartialSuccess;
                }
                else
                {
                    return ResponseStatuses.Failure;
                }
            }
        }
        /// <summary>
        /// This list contains the messages which are showed to the user for example.
        /// </summary>
        public List<string> Messages { get; set; }
        /// <summary>
        /// This list contains the warnings.
        /// </summary>
        public List<string> Warnings { get; set; }
        /// <summary>
        /// This list contains the errors for the response. When there are errors the function has been not correctly executed or it has been failed.
        /// </summary>
        public List<Error> Errors { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// The field holds the response data for the response
        /// </summary>
        public T Data { get; set; }
    }
}
