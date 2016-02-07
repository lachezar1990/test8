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
    public class ServicesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Services
        public IQueryable<Service> GetServices()
        {
            return db.Services;
        }

        // GET: api/Services/5
        public async Task<IHttpActionResult> GetService(int id)
        {
            var service = await db.Services.Where(x => x.SalonID == id && !x.Deleted)
                .Select(x => new
                {
                    Description = x.Description,
                    ImageUrl = x.ImageUrl != null ? x.ImageUrl : "friseur.png",
                    Kids = x.Kids,
                    Men = x.Men,
                    Price = x.Price,
                    SalonID = x.SalonID,
                    ServiceID = x.ServiceID,
                    ServiceName = x.ServiceName,
                    ServiceType = x.ServiceType,
                    Time = x.Time,
                    Women = x.Women
                }).ToListAsync(); //TODO: да се оптимизира

            return Ok(service);
        }

        // GET: api/Services/ForAdmin/5 използва се при админ панела
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Services/ForAdmin/{id:int}")] //SalonID
        [ResponseType(typeof(List<ServiceListDTO>))]
        public async Task<IHttpActionResult> GetServiceForAdmin(int id)
        {
            var service = await db.Services.Where(x => x.SalonID == id && !x.Deleted)
                .Select(x => new ServiceListDTO
                {
                    AddedOn = x.AddedOn,
                    Description = x.Description,
                    Kids = x.Kids,
                    Men = x.Men,
                    OrderNumber = x.OrderNumber,
                    Price = x.Price,
                    SalonID = x.SalonID,
                    ServiceID = x.ServiceID,
                    ImageUrl = x.ImageUrl,
                    ServiceName = x.ServiceName,
                    ServiceTypeID = x.TypeID,
                    ServiceTypeName = x.ServiceType.TypeName,
                    Time = x.Time,
                    Women = x.Women
                }).ToListAsync();


            return Ok(service);
        }

        // GET: api/Services/ForAdminModal/5 използва се при админ панела за прозореца
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Services/ForAdminModal/{id:int}")] //ServiceID
        public async Task<IHttpActionResult> GetServiceForAdminModal(int id)
        {
            var service = await db.Services.Where(x => x.ServiceID == id)
                .Select(x => new
                {
                    AddedOn = x.AddedOn,
                    Description = x.Description,
                    Kids = x.Kids,
                    Men = x.Men,
                    OrderNumber = x.OrderNumber,
                    Price = x.Price,
                    SalonID = x.SalonID,
                    ServiceID = x.ServiceID,
                    ImageUrl = x.ImageUrl,
                    ServiceName = x.ServiceName,
                    TypeID = x.TypeID,
                    ServiceTypeName = x.ServiceType.TypeName,
                    Time = x.Time,
                    Women = x.Women
                }).FirstOrDefaultAsync();


            return Ok(service);
        }

        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Services/ForAdminSch/{id:int}")] //salonID
        public async Task<IHttpActionResult> GetServiceForAdminSch(int id)
        {
            var service = await db.Services.Where(x => x.SalonID == id && !x.Deleted)
                .Select(x => new
                {
                    ServiceID = x.ServiceID,
                    SalonID = x.SalonID,
                    ServiceName = x.ServiceName,
                    TypeID = x.TypeID,
                    ServiceTypeName = x.ServiceType.TypeName,
                    Time = x.Time,
                    Price = x.Price
                }).ToListAsync();


            return Ok(service);
        }

        // PUT: api/Services/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceID)
            {
                return BadRequest();
            }

            Service servFromDb = await db.Services.FindAsync(id);

            servFromDb.Description = service.Description;
            servFromDb.Kids = service.Kids;
            servFromDb.Men = service.Men;
            servFromDb.Price = service.Price;
            servFromDb.ServiceName = service.ServiceName;
            servFromDb.Time = service.Time;
            servFromDb.TypeID = service.TypeID;
            servFromDb.Women = service.Women;
            servFromDb.AddedBy = service.AddedBy;

            db.Entry(servFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Service servToAdd = new Service();

            servToAdd.AddedOn = DateTime.Now;
            servToAdd.Deleted = false;
            servToAdd.Description = service.Description;
            servToAdd.ImageUrl = null;
            servToAdd.Kids = service.Kids;
            servToAdd.Men = service.Men;
            servToAdd.Price = service.Price;
            servToAdd.SalonID = service.SalonID;
            servToAdd.ServiceName = service.ServiceName;
            servToAdd.Time = service.Time;
            servToAdd.TypeID = service.TypeID;
            servToAdd.Women = service.Women;
            servToAdd.AddedBy = service.AddedBy;

            db.Services.Add(servToAdd);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = servToAdd.ServiceID }, servToAdd);
        }

        // PUT: api/Services/DeleteImage/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Services/DeleteImage/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> DelServicesImage(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceID)
            {
                return BadRequest();
            }

            Service serFromDb = await db.Services.FindAsync(id);

            serFromDb.ImageUrl = null;

            db.Entry(serFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // DELETE: api/Services/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Services/{id:int}/{username:maxlength(256)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteService(int id, string username)
        {
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            service.Deleted = true;
            service.DeletedBy = username;
            service.DeletedDate = DateTime.Now;

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

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceID == id) > 0;
        }
    }
}