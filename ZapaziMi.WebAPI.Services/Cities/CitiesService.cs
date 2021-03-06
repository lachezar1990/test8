﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Cities;
using ZapaziMi.DAL.Entities.Cities;
using ZapaziMi.WebAPI.Services.Models;
using ZapaziMi.WebAPI.Services.Models.Enums;

namespace ZapaziMi.WebAPI.Services.Cities
{
    public class CitiesService : ICitiesService
    {
        private ICitiesDAL _citiesDal;

        public CitiesService(ICitiesDAL citiesDal)
        {
            _citiesDal = citiesDal;
        }

        public async Task<List<GetCityEntity>> GetCities()
        {
            return await _citiesDal.GetCities();
        }

        public async Task<ResponseModel<GetCityEntity>> GetCityById(int id)
        {
            ResponseModel<GetCityEntity> result = new ResponseModel<GetCityEntity>();
            GetCityEntity city = await _citiesDal.GetCityById(id);
            if (city == null)
            {
                result.Errors.Add(new Error()
                {
                    ErrorType = ErrorTypes.NotFound,
                    ShortMessage = "The city hasn't been found."
                });
                return result;
            }

            result.Data = city;

            return result;
        }
    }
}
