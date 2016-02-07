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
    public class EmplWithPositionsController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/EmplWithPositions
        public IQueryable<EmplWithPosition> GetEmplWithPositions()
        {
            return db.EmplWithPositions;
        }

        // GET: api/EmplWithPositions/5
        [ResponseType(typeof(List<EmplWithPosition>))]
        public async Task<IHttpActionResult> GetEmplWithPosition(int id)
        {
            List<EmplWithPosition> emplWithPosition = await db.EmplWithPositions
                .Where(x => x.SalonID == id).OrderBy(x => x.FullName).ToListAsync();
            
            return Ok(emplWithPosition);
        }

        // PUT: api/EmplWithPositions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmplWithPosition(int id, EmplWithPosition emplWithPosition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emplWithPosition.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(emplWithPosition).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmplWithPositionExists(id))
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

        // POST: api/EmplWithPositions
        [ResponseType(typeof(EmplWithPosition))]
        public async Task<IHttpActionResult> PostEmplWithPosition(EmplWithPosition emplWithPosition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmplWithPositions.Add(emplWithPosition);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmplWithPositionExists(emplWithPosition.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = emplWithPosition.EmployeeID }, emplWithPosition);
        }

        // DELETE: api/EmplWithPositions/5
        [ResponseType(typeof(EmplWithPosition))]
        public async Task<IHttpActionResult> DeleteEmplWithPosition(int id)
        {
            EmplWithPosition emplWithPosition = await db.EmplWithPositions.FindAsync(id);
            if (emplWithPosition == null)
            {
                return NotFound();
            }

            db.EmplWithPositions.Remove(emplWithPosition);
            await db.SaveChangesAsync();

            return Ok(emplWithPosition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmplWithPositionExists(int id)
        {
            return db.EmplWithPositions.Count(e => e.EmployeeID == id) > 0;
        }
    }
}