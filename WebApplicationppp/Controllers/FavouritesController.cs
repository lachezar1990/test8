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

namespace WebApplicationppp.Controllers
{
    public class FavouritesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Favourites
        public IQueryable<Favourite> GetFavourites()
        {
            return db.Favourites;
        }

        // GET: api/Favourites/username
        [Route("api/Favourites/{username:maxlength(256)}")]
        public async Task<IHttpActionResult> GetFavourite(string username)
        {
            var result = await db.SalonsMainScreenMobiles.Join(db.Favourites,
                x => x.SalonID,
                y => y.SalonID,
                (x, y) => new { x, y })
                .Where(x => x.y.CreateBy == username)
                .Select(x => new
            {
                SalonID = x.x.SalonID,
                SalonName = x.x.SalonName,
                ImagePath = x.x.ImagePath,
                City = x.x.CityName,
                Neighbourhood = x.x.NeighbourhoodName,
                Apartment = x.x.Apartment,
                Entrance = x.x.Entrance,
                Flat = x.x.Flat,
                Number = x.x.Number,
                Street = x.x.Street,
                Rating = x.x.Rating,
                Favourite = true,
                FavouriteID = x.y.FavouriteID,
                CommentsCount = (db.Ratings.Where(y => y.SalonID == x.x.SalonID && !y.IsDeleted).Count())
            }).ToListAsync();

            return Ok(result);
        }

        // PUT: api/Favourites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favourite.FavouriteID)
            {
                return BadRequest();
            }

            db.Entry(favourite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteExists(id))
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

        // POST: api/Favourites
        [ResponseType(typeof(Favourite))]
        public async Task<IHttpActionResult> PostFavourite(Favourite favourite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            favourite.AddedOn = DateTime.Now;

            db.Favourites.Add(favourite);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = favourite.FavouriteID }, favourite);
        }

        // DELETE: api/Favourites/5
        [ResponseType(typeof(void))]
        [Route("api/Favourites/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFavourite(int id)
        {
            Favourite favourite = await db.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            db.Favourites.Remove(favourite);
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FavouriteExists(int id)
        {
            return db.Favourites.Count(e => e.FavouriteID == id) > 0;
        }
    }
}