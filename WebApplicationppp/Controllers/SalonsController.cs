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
    public class SalonsController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Salons
        public IQueryable<Salon> GetSalons()
        {
            return db.Salons;
        }

        // GET: api/Salons/5
        // TODO: да се направи с потребителско име 
        [Route("api/Salons/{username:maxlength(256)}")]
        public async Task<IHttpActionResult> GetSalon(string username)
        {
            var salons = await db.Salons.Where(x => x.CreateBy.ToUpper() == username.ToUpper()).Select(x => new
            {
                SalonID = x.SalonID,
                SalonName = x.SalonName
            }).ToListAsync();

            return Ok(salons);
        }

        // GET: api/Salons/5
        // TODO: да се направи с потребителско име 
        [Route("api/GetSalon/{id:int}")]
        public async Task<IHttpActionResult> GetSalon(int id)
        {
            var salons = await db.Salons.Join(db.Addresses,
                x => x.AddressID,
                y => y.AddressID,
                (x, y) => new { x, y })
                .Where(x => x.x.SalonID == id).Select(x => new
                {
                    SalonID = x.x.SalonID,
                    SalonName = x.x.SalonName,
                    CityID = x.y.CityID,
                    NeighbourhoodID = x.y.NeighbourhoodID,
                    Neighbourhoods = db.Neighbourhoods.Where(z => z.CityID == x.y.CityID).Select(z => new
                    {
                        NeighbourhoodID = z.NeighbourhoodID,
                        NeighbourhoodName = z.NeighbourhoodName
                    }),
                    SalonStreet = x.y.Street,
                    SalonFlat = x.y.Flat,
                    SalonEntrance = x.y.Entrance,
                    SalonNumber = x.y.Number,
                    SalonApartment = x.y.Apartment,
                    SiteUrl = x.x.SiteUrl,
                    Email = x.x.Emails.Where(z => z.SalonID == x.x.SalonID).Select(z => z.Email1).FirstOrDefault(),
                    SalonDescription = x.x.Description,
                    VisibleForUsers = x.x.VisibleForUsers,
                    SalonPhones = x.x.Phones.Select(z => z.Phone1),
                    SalonSchedule = x.x.SalonSchedules.Where(z =>
                    !z.Date.HasValue).Select(z => new
                    {
                        Date = z.Date,
                        DayOfWeek = z.DayOfWeek,
                        EndTime = z.EndTime,
                        StartTime = z.StartTime,
                        Holiday = z.Holiday,
                        SalonID = z.SalonID,
                        ScheduleID = z.ScheduleID
                    }).OrderBy(z => z.DayOfWeek),
                    SalonImages = x.x.SalonImages.Where(z => !z.IsDeleted).Select(z => new
                    {
                        ImageID = z.ImageID,
                        ImagePath = z.ImagePath,
                        ImageName = z.ImageName,
                        IsMain = z.IsMain
                    }),
                    AdminMessage = x.x.AdminMessage
                }).FirstOrDefaultAsync();

            return Ok(salons);
        }

        // PUT: api/Salons/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Salons/{id:int}")]
        public async Task<IHttpActionResult> PutSalon(int id, SalonForUpdate salon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salon.SalonID)
            {
                return BadRequest();
            }

            Salon salonFromDb = await db.Salons.FindAsync(id);

            salonFromDb.Description = salon.SalonDescription;
            salonFromDb.SalonName = salon.SalonName;
            salonFromDb.SiteUrl = salon.SiteUrl;
            salonFromDb.VisibleForUsers = salon.VisibleForUsers;
            salonFromDb.Address.CityID = salon.CityID;
            salonFromDb.Address.NeighbourhoodID = salon.NeighbourhoodID;
            salonFromDb.Address.Street = salon.SalonStreet;
            salonFromDb.Address.Flat = salon.SalonFlat;
            salonFromDb.Address.Number = salon.SalonNumber;
            salonFromDb.Address.Entrance = salon.SalonEntrance;
            salonFromDb.Address.Apartment = salon.SalonApartment;
            Email salonEmail = await db.Emails.Where(x => x.SalonID == id).FirstOrDefaultAsync();

            if (salonEmail != null)
            {
                salonEmail.Email1 = salon.Email;
            }
            else
            {
                salonEmail = new Email();
                salonEmail.Email1 = salon.Email;
                salonEmail.SalonID = salonFromDb.SalonID;
                db.Emails.Add(salonEmail);
            }

            //телефони

            string[] phones = salon.SalonPhones.Split(',');

            List<Phone> phonesFromDb = await db.Phones.Where(x => x.SalonID == id).ToListAsync();

            db.Phones.RemoveRange(phonesFromDb);

            List<Phone> itemsToAdd = phones.Select(x => new Phone
            {
                Phone1 = x,
                SalonID = id
            }).ToList();

            db.Phones.AddRange(itemsToAdd);
            if (salonFromDb.SalonSchedules.Count == 7)
            {
                int i = 0;
                foreach (var day in salonFromDb.SalonSchedules)
                {
                    day.Holiday = salon.SalonSchedule[i].Holiday;
                    day.StartTime = salon.SalonSchedule[i].StartTime;
                    day.EndTime = salon.SalonSchedule[i].EndTime;
                    i++;
                }
            }
            else
            {
                List<SalonSchedule> schedulesToAdd = new List<SalonSchedule>();
                foreach (var day in salon.SalonSchedule)
                {
                    schedulesToAdd.Add(new SalonSchedule
                    {
                        DayOfWeek = day.DayOfWeek,
                        EndTime = day.EndTime,
                        StartTime = day.StartTime,
                        Holiday = day.Holiday,
                        SalonID = id
                    });
                }

                salonFromDb.SalonSchedules = schedulesToAdd;
            }

            db.Entry(salonFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(id))
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

        // POST: api/Salons
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Salon))]
        public async Task<IHttpActionResult> PostSalon(SalonForUpdate salon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Salon newSalon = new Salon();
            newSalon.AddedOn = DateTime.Now;
            newSalon.Address = new Address();
            newSalon.Address.AddressType = 2;
            newSalon.Address.Apartment = salon.SalonApartment;
            newSalon.Address.CityID = salon.CityID;
            newSalon.Address.Entrance = salon.SalonEntrance;
            newSalon.Address.Flat = salon.SalonFlat;
            newSalon.Address.NeighbourhoodID = salon.NeighbourhoodID;
            newSalon.Address.Number = salon.SalonNumber;
            newSalon.Address.Street = salon.SalonStreet;
            newSalon.CompanyID = await db.Companies.Where(x => x.CreateBy.ToUpper() == salon.Username)
                .Select(x => x.CompanyID).FirstOrDefaultAsync();
            newSalon.CreateBy = salon.Username;
            newSalon.Description = salon.SalonDescription;
            newSalon.SalonName = salon.SalonName;
            newSalon.SiteUrl = salon.SiteUrl;
            newSalon.VisibleForUsers = salon.VisibleForUsers;
            newSalon.IsDeleted = false;

            List<Email> emails = new List<Email>();
            emails.Add(new Email
            {
                Email1 = salon.Email
            });
            newSalon.Emails = emails;
            string[] phones = salon.SalonPhones.Split(',');
            List<Phone> itemsToAdd = phones.Select(x => new Phone
            {
                Phone1 = x
            }).ToList();

            newSalon.Phones = itemsToAdd;

            List<SalonSchedule> schedulesToAdd = new List<SalonSchedule>();
            foreach (var day in salon.SalonSchedule)
            {
                schedulesToAdd.Add(new SalonSchedule
                {
                    DayOfWeek = day.DayOfWeek,
                    EndTime = day.EndTime,
                    StartTime = day.StartTime,
                    Holiday = day.Holiday
                });
            }

            newSalon.SalonSchedules = schedulesToAdd;

            db.Salons.Add(newSalon);
            await db.SaveChangesAsync();

            List<Salon> salonForReturn = new List<Salon>();
            salonForReturn.Add(newSalon);

            var salonForRet = salonForReturn.Select(x => new
            {
                SalonID = x.SalonID,
                SalonName = x.SalonName,
                CityID = x.Address.CityID,
                NeighbourhoodID = x.Address.NeighbourhoodID,
                Neighbourhoods = db.Neighbourhoods.Where(z => z.CityID == x.Address.CityID).Select(z => new
                {
                    NeighbourhoodID = z.NeighbourhoodID,
                    NeighbourhoodName = z.NeighbourhoodName
                }),
                SalonStreet = x.Address.Street,
                SalonFlat = x.Address.Flat,
                SalonEntrance = x.Address.Entrance,
                SalonNumber = x.Address.Number,
                SalonApartment = x.Address.Apartment,
                SiteUrl = x.SiteUrl,
                Email = x.Emails.Where(z => z.SalonID == x.SalonID).Select(z => z.Email1).FirstOrDefault(),
                SalonDescription = x.Description,
                VisibleForUsers = x.VisibleForUsers,
                SalonPhones = x.Phones.Select(z => z.Phone1),
                SalonSchedule = x.SalonSchedules.Where(z =>
                !z.Date.HasValue).Select(z => new
                {
                    Date = z.Date,
                    DayOfWeek = z.DayOfWeek,
                    EndTime = z.EndTime,
                    StartTime = z.StartTime,
                    Holiday = z.Holiday,
                    SalonID = z.SalonID,
                    ScheduleID = z.ScheduleID
                }).OrderBy(z => z.DayOfWeek),
                SalonImages = x.SalonImages.Where(z => !z.IsDeleted).Select(z => new
                {
                    ImageID = z.ImageID,
                    ImagePath = z.ImagePath,
                    ImageName = z.ImageName,
                    IsMain = z.IsMain
                })
            }).FirstOrDefault();

            return CreatedAtRoute("DefaultApi", new { id = newSalon.SalonID }, salonForRet);
        }

        // DELETE: api/Salons/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Salon))]
        public async Task<IHttpActionResult> DeleteSalon(int id)
        {
            Salon salon = await db.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            db.Salons.Remove(salon);
            await db.SaveChangesAsync();

            return Ok(salon);
        }

        #region Admin

        [Authorize(Roles = "Admin")]
        [Route("api/SalonsForAdmin/{username:maxlength(256)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalonsAdmin(string username)
        {
            var salons = await db.Salons.Where(x => x.CreateBy.ToLower() == username.ToLower()).Select(x => new
            {
                SalonID = x.SalonID,
                SalonName = x.SalonName,
                AdminMessage = x.AdminMessage,
                VisibleForUsers = x.VisibleForUsers,
                AddedOn = x.AddedOn
            }).ToListAsync();

            return Ok(salons);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/SalonsForAdmin/ChangeVisibility/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> GetSalonsAdmin(int id, Salon salon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salonFromDb = await db.Salons.FindAsync(id);

            if (salonFromDb != null)
            {
                salonFromDb.VisibleForUsers = salon.VisibleForUsers;

                await db.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/SalonsForAdmin/AddMessage/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> GetSalonsRealAdmin(int id, Salon salon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salonFromDb = await db.Salons.FindAsync(id);

            if (salonFromDb != null)
            {
                salonFromDb.AdminMessage = salon.AdminMessage;

                await db.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalonExists(int id)
        {
            return db.Salons.Count(e => e.SalonID == id) > 0;
        }
    }
}