using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Cities;

namespace ZapaziMi.DAL.Cities
{
    public interface ICitiesDAL
    {
        Task<List<GetCityEntity>> GetCities();
        Task<GetCityEntity> GetCityById(int id);
    }
}
