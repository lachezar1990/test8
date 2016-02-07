using ImageResizer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationppp.Models;

namespace WebApplicationppp.Controllers
{
    public class SalonImagesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/SalonImages
        public IQueryable<SalonImage> GetSalonImages()
        {
            return db.SalonImages;
        }

        // GET: api/SalonImages/5
        public async Task<IHttpActionResult> GetSalonImage(int id)
        {
            var salonImage = await db.SalonImages.Where(x => x.SalonID == id && !x.IsDeleted).Select(x => new
            {
                ImageID = x.ImageID,
                ImagePath = x.ImagePath,
                ImageName = x.ImageName,
                IsMain = x.IsMain
            }).ToListAsync();

            return Ok(salonImage);
        }

        // PUT: api/SalonImages/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSalonImage(int id, SalonImageForDelete salonImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salonImage.ImageID)
            {
                return BadRequest();
            }

            SalonImage imageFromDb = await db.SalonImages.FindAsync(id);

            if (salonImage.ForDelete)
            {
                imageFromDb.IsDeleted = true;
                imageFromDb.DeleteBy = salonImage.Username;
                imageFromDb.DeletedDate = DateTime.Now;
            }
            else
            {
                imageFromDb.IsMain = true;

                SalonImage lastMain = await db.SalonImages.Where(x => x.SalonID == imageFromDb.SalonID && x.IsMain).FirstOrDefaultAsync();

                if (lastMain != null)
                {
                    lastMain.IsMain = false;
                    db.Entry(lastMain).State = EntityState.Modified;
                }
            }

            db.Entry(imageFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonImageExists(id))
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

        // POST: api/SalonImages
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(SalonImage))]
        public async Task<IHttpActionResult> PostSalonImage(SalonImage salonImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalonImages.Add(salonImage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = salonImage.ImageID }, salonImage);
        }

        // DELETE: api/SalonImages/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(SalonImage))]
        public async Task<IHttpActionResult> DeleteSalonImage(int id)
        {
            SalonImage salonImage = await db.SalonImages.FindAsync(id);
            if (salonImage == null)
            {
                return NotFound();
            }

            db.SalonImages.Remove(salonImage);
            await db.SaveChangesAsync();

            return Ok(salonImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalonImageExists(int id)
        {
            return db.SalonImages.Count(e => e.ImageID == id) > 0;
        }
    }
}