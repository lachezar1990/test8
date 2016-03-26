using System.Collections.Generic;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Cities;
using ZapaziMi.WebAPI.Services.Models;

namespace ZapaziMi.WebAPI.Services.Cities
{
    public interface ICitiesService
    {
        Task<List<GetCityEntity>> GetCities();
        Task<ResponseModel<GetCityEntity>> GetCityById(int id);
    }
}
