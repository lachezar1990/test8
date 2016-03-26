using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Cities;
using System.Data.Entity;
using ZapaziMi.DAL.Entities.ZapaziMiDb;

namespace ZapaziMi.DAL.Cities
{
    public class CitiesDAL : ICitiesDAL
    {
        private Diplomna_newEntities db = new Diplomna_newEntities();

        public async Task<List<GetCityEntity>> GetCities()
        {
            List<GetCityEntity> cities = await db.Cities.Select(x => new GetCityEntity
            {
                CityId = x.CityID,
                CityName = x.CityName
            }).ToListAsync();

            return cities;
        }

        public async Task<GetCityEntity> GetCityById(int id)
        {
            GetCityEntity city = await db.Cities
                .Where(x => x.CityID == id)
                .Select(x => new GetCityEntity
            {
                CityId = x.CityID,
                CityName = x.CityName
            }).FirstOrDefaultAsync();

            return city;
        }
    }
}
