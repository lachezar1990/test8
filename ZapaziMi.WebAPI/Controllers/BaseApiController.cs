using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplicationppp.Results;
using ZapaziMi.WebAPI.Services.Models;

namespace WebApplicationppp.Controllers
{
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// Gets the result and return it with Ok status.
        /// </summary>
        /// <typeparam name="T">Could be only async task method</typeparam>
        /// <param name="data">Input param for the method from the service</param>
        /// <returns>ResponseModel</returns>
        public async Task<IHttpActionResult> GetMyResult<T>(Func<Task<T>> data)
        {
            T returnData = await data.Invoke();
            return Ok(returnData);
        }

        /// <summary>
        /// Checks the model and whether everything is OK gets the result and return it with Ok status. 
        /// </summary>
        /// <typeparam name="T">Could be only async task method</typeparam>
        /// <param name="data">Input param for the method from the service</param>
        /// <returns>ResponseModel</returns>
        public async Task<IHttpActionResult> GetMyResultWithModelValidation<T>(Func<Task<T>> data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            T returnData = await data.Invoke();
            return Ok(returnData);
        }

        /// <summary>
        /// Checks the model and whether everything is OK gets the result and return it with Created at route (201) status. 
        /// </summary>
        /// <typeparam name="T">Could be only async task method</typeparam>
        /// <param name="data">Input param for the method from the service</param>
        /// <returns>ResponseModel</returns>
        public async Task<IHttpActionResult> GetMyInsertResultWithModelValidation<T>(Func<Task<Tuple<int, T>>> data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tuple<int, T> returnData = await data.Invoke();
            return CreatedAtRoute("DefaultApi", new { id = returnData.Item1 }, returnData.Item2);
        }

        /// <summary>
        /// Used for void methods.
        /// </summary>
        /// <param name="data">Input param ResponseModel from void methods</param>
        /// <returns>ResponseModel</returns>
        public async Task<IHttpActionResult> GetMyResult(Func<Task<ResponseModel>> data)
        {
            ResponseModel result = await data.Invoke();
            return Ok(result);
        }

        /// <summary>
        /// used for void methods with validation
        /// </summary>
        /// <param name="data">Input param ResponseModel from void methods</param>
        /// <returns>ResponseModel</returns>
        public async Task<IHttpActionResult> GetMyResultWithModelValidation(Func<Task<ResponseModel>> data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseModel returnData = await data.Invoke();

            switch (returnData.ResponseStatus)
            {
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.Success:
                    return Ok(returnData);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.PartialSuccess:
                    return Ok(returnData); //may we have to refactor this?
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.BadRequest:
                    return new BadRequestActionResult<ResponseModel>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.Unauthorized:
                    return new UnauthorizedActionResult<ResponseModel>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.NotFound:
                    return new NotFoundActionResult<ResponseModel>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.InternalServerError:
                    return new InternalServerErrorActionResult<ResponseModel>(returnData, Request);
                default:
                    return Ok(returnData);
            }
        }

        /// <summary>
        /// used for methods with response model and returns the data with the status of the execution
        /// </summary>
        /// <typeparam name="T">The type of the response</typeparam>
        /// <param name="data">The data service</param>
        /// <returns>ResponseModel with status</returns>
        public async Task<IHttpActionResult> GetMyResult<T>(Func<Task<ResponseModel<T>>> data)
        {
            ResponseModel<T> returnData = await data.Invoke();

            switch (returnData.ResponseStatus)
            {
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.Success:
                    return Ok(returnData);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.PartialSuccess:
                    return Ok(returnData); //may we have to refactor this?
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.BadRequest:
                    return new BadRequestActionResult<ResponseModel<T>>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.Unauthorized:
                    return new UnauthorizedActionResult<ResponseModel<T>>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.NotFound:
                    return new NotFoundActionResult<ResponseModel<T>>(returnData, Request);
                case ZapaziMi.WebAPI.Services.Models.Enums.ResponseStatuses.InternalServerError:
                    return new InternalServerErrorActionResult<ResponseModel<T>>(returnData, Request);
                default:
                    return Ok(returnData);
            }
        }
    }
}
