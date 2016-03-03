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
    public class ServicesForDetailsPagesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/ServicesForDetailsPages
        public IQueryable<ServicesForDetailsPage> GetServicesForDetailsPages()
        {
            return db.ServicesForDetailsPages;
        }

        // GET: api/ServicesForDetailsPages/5
        //[ResponseType(typeof(ServicesForDetailsPage))]
        public async Task<IHttpActionResult> GetServicesForDetailsPage(int id)
        {
            var servicesForDetailsPage = await db.ServicesForDetailsPages
                .Where(x => x.SalonID == id).ToListAsync();
            if (servicesForDetailsPage == null)
            {
                return NotFound();
            }

            return Ok(servicesForDetailsPage);
        }

        // PUT: api/ServicesForDetailsPages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServicesForDetailsPage(int id, ServicesForDetailsPage servicesForDetailsPage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicesForDetailsPage.ServiceID)
            {
                return BadRequest();
            }

            db.Entry(servicesForDetailsPage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesForDetailsPageExists(id))
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

        // POST: api/ServicesForDetailsPages
        [ResponseType(typeof(ServicesForDetailsPage))]
        public async Task<IHttpActionResult> PostServicesForDetailsPage(ServicesForDetailsPage servicesForDetailsPage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServicesForDetailsPages.Add(servicesForDetailsPage);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServicesForDetailsPageExists(servicesForDetailsPage.ServiceID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = servicesForDetailsPage.ServiceID }, servicesForDetailsPage);
        }

        // DELETE: api/ServicesForDetailsPages/5
        [ResponseType(typeof(ServicesForDetailsPage))]
        public async Task<IHttpActionResult> DeleteServicesForDetailsPage(int id)
        {
            ServicesForDetailsPage servicesForDetailsPage = await db.ServicesForDetailsPages.FindAsync(id);
            if (servicesForDetailsPage == null)
            {
                return NotFound();
            }

            db.ServicesForDetailsPages.Remove(servicesForDetailsPage);
            await db.SaveChangesAsync();

            return Ok(servicesForDetailsPage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServicesForDetailsPageExists(int id)
        {
            return db.ServicesForDetailsPages.Count(e => e.ServiceID == id) > 0;
        }
    }
}