﻿using System;
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

            ResponseModel result = await data.Invoke();
            return Ok(result);
        }

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
