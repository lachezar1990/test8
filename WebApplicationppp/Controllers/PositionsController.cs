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
    public class PositionsController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Positions
        public IQueryable<Position> GetPositions()
        {
            return db.Positions;
        }

        // GET: api/Positions/5
        [Authorize(Roles = "SalonAdmin")]
        public async Task<IHttpActionResult> GetPosition(int id)
        {
            var positions = await db.Positions.Where(x => !x.Deleted && x.SalonID == id).Select(x => new
            {
                PositionID = x.PositionID,
                PositionName = x.PositionName,
                SalonID = x.SalonID,
                AddedOn = x.AddedOn
            }).ToListAsync();

            return Ok(positions);
        }

        // PUT: api/Positions/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPosition(int id, PositionForDelete position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.PositionID)
            {
                return BadRequest();
            }

            Position positionFromDb = await db.Positions.FindAsync(id);

            if (position.IsForDelete)
            {
                positionFromDb.Deleted = true;
                positionFromDb.DeletedBy = position.UserName;
                positionFromDb.DeletedDate = DateTime.Now;
            }
            else
            {
                positionFromDb.PositionName = position.PositionName;
            }

            db.Entry(positionFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
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

        // POST: api/Positions
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> PostPosition(PositionToAddDTO position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Position positionToAdd = new Position()
            {
                Deleted = false,
                PositionName = position.PositionName,
                SalonID = position.SalonID,
                AddedOn = DateTime.Now
            };

            db.Positions.Add(positionToAdd);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = positionToAdd.PositionID }, positionToAdd);
        }

        // DELETE: api/Positions/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Position))]
        public async Task<IHttpActionResult> DeletePosition(int id)
        {
            Position position = await db.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            db.Positions.Remove(position);
            await db.SaveChangesAsync();

            return Ok(position);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return db.Positions.Count(e => e.PositionID == id) > 0;
        }
    }
}