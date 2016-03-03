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
    public class NeighbourhoodsController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Neighbourhoods
        public IQueryable<Neighbourhood> GetNeighbourhoods()
        {
            return db.Neighbourhoods;
        }

        // GET: api/Neighbourhoods/5

        public async Task<IHttpActionResult> GetNeighbourhood(int id)
        {
            var neighbourhoods = await db.Neighbourhoods.Where(x => x.CityID == id).Select(x => new
            {
                NeighbourhoodID = x.NeighbourhoodID,
                NeighbourhoodName = x.NeighbourhoodName,
                CityID = x.CityID
            }).ToListAsync();

            return Ok(neighbourhoods);
        }

        // PUT: api/Neighbourhoods/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNeighbourhood(int id, Neighbourhood neighbourhood)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != neighbourhood.NeighbourhoodID)
            {
                return BadRequest();
            }

            db.Entry(neighbourhood).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NeighbourhoodExists(id))
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

        // POST: api/Neighbourhoods
        [ResponseType(typeof(Neighbourhood))]
        public async Task<IHttpActionResult> PostNeighbourhood(Neighbourhood neighbourhood)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Neighbourhoods.Add(neighbourhood);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = neighbourhood.NeighbourhoodID }, neighbourhood);
        }

        // DELETE: api/Neighbourhoods/5
        [ResponseType(typeof(Neighbourhood))]
        public async Task<IHttpActionResult> DeleteNeighbourhood(int id)
        {
            Neighbourhood neighbourhood = await db.Neighbourhoods.FindAsync(id);
            if (neighbourhood == null)
            {
                return NotFound();
            }

            db.Neighbourhoods.Remove(neighbourhood);
            await db.SaveChangesAsync();

            return Ok(neighbourhood);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NeighbourhoodExists(int id)
        {
            return db.Neighbourhoods.Count(e => e.NeighbourhoodID == id) > 0;
        }
    }
}