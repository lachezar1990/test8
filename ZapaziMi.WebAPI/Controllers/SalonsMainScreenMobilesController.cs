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
using System.Web.Http.Cors;
using System.Linq.Expressions;
using System.Web;
using System.Net.Http.Headers;

namespace WebApplicationppp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SalonsMainScreenMobilesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "Has-More")]
        // GET: api/SalonsMainScreenMobiles
        [Route("api/SalonsMainScreenMobiles/{cityId:int}/{nId:int}/{page:int}/{username:maxlength(256)?}")]
        public async Task<IHttpActionResult> GetSalonsMainScreenMobiles(int cityId, int nId, int page, string username = "")
        {
            Expression<Func<SalonsMainScreenMobile, bool>> where;

            if (nId != 0)
            {
                where = x => x.CityID == cityId && x.NeighbourhoodID == nId && x.VisibleForUsers;
            }
            else
            {
                where = x => x.CityID == cityId && x.VisibleForUsers;
            }

            int pageSize = 10;
            int forSkip = ((page - 1) * pageSize);

            var result = await db.SalonsMainScreenMobiles.Where(where).Select(x => new
            {
                SalonID = x.SalonID,
                SalonName = x.SalonName,
                ImagePath = x.ImagePath,
                City = x.CityName,
                Neighbourhood = x.NeighbourhoodName,
                Apartment = x.Apartment,
                Entrance = x.Entrance,
                Flat = x.Flat,
                Number = x.Number,
                Street = x.Street,
                Rating = x.Rating,
                Favourite = username != "" ? db.Favourites.Any(y => y.SalonID == x.SalonID && y.CreateBy == username) : false,
                CommentsCount = (db.Ratings.Where(y => y.SalonID == x.SalonID && !y.IsDeleted).Count())
            }).OrderBy(x => x.SalonID).Skip(forSkip).Take(pageSize).ToListAsync();


            HttpContext.Current.Response.AddHeader("Has-More", (result.Count == 10 ? "1" : "0"));


            return Ok(result);
        }

        // GET: api/SalonsMainScreenMobiles/5
        //[ResponseType(typeof(SalonsMainScreenMobile))]
        public async Task<IHttpActionResult> GetSalonsMainScreenMobile(int id, [FromUri]string username)
        {
            var salonsDetails = await db.SalonDetailsInfoes.Select(x => new
            {
                SalonID = x.SalonID,
                SalonName = x.SalonName,
                Description = x.Description,
                Phones = x.Phones,
                Emails = x.Emails,
                SiteUrl = x.SiteUrl,
                ImagePath = x.ImagePath,
                City = x.CityName,
                Neighbourhood = x.NeighbourhoodName,
                Apartment = x.Apartment,
                Entrance = x.Entrance,
                Flat = x.Flat,
                Number = x.Number,
                Street = x.Street,
                Rating = x.Rating,
                Comments = (db.Ratings.Where(y => y.SalonID == id && !y.IsDeleted).Select(y => new
                {
                    AddedOn = y.AddedOn,
                    Comment = y.Comment,
                    CreateBy = y.CreateBy,
                    Rating1 = y.Rating1
                }).OrderByDescending(z => z.AddedOn)).ToList(),
                Images = db.SalonImages.Where(y => y.SalonID == x.SalonID && !y.IsDeleted).OrderByDescending(y => y.IsMain)
                .ThenByDescending(y => y.AddedOn)
                .Select(y => new
                {
                    ImageName = y.ImageName,
                    ImagePath = y.ImagePath,
                    ImageID = y.ImageID
                }),
                ServicesCount = x.ServicesCount,
                SalonSchedule = db.SalonSchedules.Where(y => y.SalonID == id
                    && !y.Date.HasValue && !y.Holiday)
                    .Select(y => new
                    {
                        Date = y.Date,
                        DayOfWeek = y.DayOfWeek,
                        EndTime = y.EndTime,
                        Holiday = y.Holiday,
                        SalonID = y.SalonID,
                        ScheduleID = y.ScheduleID,
                        StartTime = y.StartTime
                    })
                    .OrderBy(y => y.DayOfWeek),
                Favourite = db.Favourites.Any(y => y.SalonID == x.SalonID && y.CreateBy == username)
            }).Where(x => x.SalonID == id).FirstOrDefaultAsync();

            if (salonsDetails == null)
            {
                return NotFound();
            }

            return Ok(salonsDetails);
        }

        // PUT: api/SalonsMainScreenMobiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalonsMainScreenMobile(int id, SalonsMainScreenMobile salonsMainScreenMobile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salonsMainScreenMobile.SalonID)
            {
                return BadRequest();
            }

            db.Entry(salonsMainScreenMobile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonsMainScreenMobileExists(id))
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

        // POST: api/SalonsMainScreenMobiles
        [ResponseType(typeof(SalonsMainScreenMobile))]
        public async Task<IHttpActionResult> PostSalonsMainScreenMobile(SalonsMainScreenMobile salonsMainScreenMobile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalonsMainScreenMobiles.Add(salonsMainScreenMobile);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalonsMainScreenMobileExists(salonsMainScreenMobile.SalonID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = salonsMainScreenMobile.SalonID }, salonsMainScreenMobile);
        }

        // DELETE: api/SalonsMainScreenMobiles/5
        [ResponseType(typeof(SalonsMainScreenMobile))]
        public async Task<IHttpActionResult> DeleteSalonsMainScreenMobile(int id)
        {
            SalonsMainScreenMobile salonsMainScreenMobile = await db.SalonsMainScreenMobiles.FindAsync(id);
            if (salonsMainScreenMobile == null)
            {
                return NotFound();
            }

            db.SalonsMainScreenMobiles.Remove(salonsMainScreenMobile);
            await db.SaveChangesAsync();

            return Ok(salonsMainScreenMobile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalonsMainScreenMobileExists(int id)
        {
            return db.SalonsMainScreenMobiles.Count(e => e.SalonID == id) > 0;
        }
    }
}