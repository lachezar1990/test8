using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Cities;
using ZapaziMi.DAL.Entities.Cities;

namespace ZapaziMi.WebAPI.Services.Cities
{
    public class CitiesService : ICitiesService
    {
        private ICitiesDAL citiesDal;

        public CitiesService()
        {
            citiesDal = new CitiesDAL();
        }

        public async Task<List<GetCityEntity>> GetCities()
        {
            return await citiesDal.GetCities();
        }
    }
}
