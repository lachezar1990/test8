using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationppp.Models;
using ZapaziMi.DAL.Entities.Cities;
using ZapaziMi.WebAPI.Services.Cities;

namespace WebApplicationppp.Controllers
{
    [RoutePrefix("api/Cities")]
    public class CitiesController : BaseApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();
        private ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        // GET: api/Cities
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<GetCityEntity>))]
        public async Task<IHttpActionResult> GetCities()
        {
            return await GetMyResult(() => _citiesService.GetCities());
        }

        // GET: api/Cities/5
        [Route("{id:int}")]
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> GetCity(int id)
        {
            return await GetMyResult(() => _citiesService.GetCityById(id));
        }

        // PUT: api/Cities/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCity(int id, City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.CityID)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cities
        [HttpPost]
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cities.Add(city);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = city.CityID }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete]
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> DeleteCity(int id)
        {
            City city = await db.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            db.Cities.Remove(city);
            await db.SaveChangesAsync();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(int id)
        {
            return db.Cities.Count(e => e.CityID == id) > 0;
        }
    }
}