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
    public class ServiceTypesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/ServiceTypes
        public async Task<IHttpActionResult> GetServiceTypes()
        {
            return Ok(await db.ServiceTypes.Select(x => new { x.TypeID, x.TypeName }).ToListAsync());
        }

        // GET: api/ServiceTypes/5
        [ResponseType(typeof(ServiceType))]
        public async Task<IHttpActionResult> GetServiceType(int id)
        {
            ServiceType serviceType = await db.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return NotFound();
            }

            return Ok(serviceType);
        }

        // PUT: api/ServiceTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServiceType(int id, ServiceType serviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceType.TypeID)
            {
                return BadRequest();
            }

            db.Entry(serviceType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceTypeExists(id))
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

        // POST: api/ServiceTypes
        [ResponseType(typeof(ServiceType))]
        public async Task<IHttpActionResult> PostServiceType(ServiceType serviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServiceTypes.Add(serviceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServiceTypeExists(serviceType.TypeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = serviceType.TypeID }, serviceType);
        }

        // DELETE: api/ServiceTypes/5
        [ResponseType(typeof(ServiceType))]
        public async Task<IHttpActionResult> DeleteServiceType(int id)
        {
            ServiceType serviceType = await db.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return NotFound();
            }

            db.ServiceTypes.Remove(serviceType);
            await db.SaveChangesAsync();

            return Ok(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceTypeExists(int id)
        {
            return db.ServiceTypes.Count(e => e.TypeID == id) > 0;
        }
    }
}