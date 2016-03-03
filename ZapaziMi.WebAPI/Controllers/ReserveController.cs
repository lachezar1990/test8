using WebApplicationppp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApplicationppp.Controllers
{
    public class ReserveController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();
        private static DateTimesLocal localTime = new DateTimesLocal();
        private DateTime getTime = localTime.DateTimeNowLocal(); //локално време заради американския сървър

        // GET: api/Reserve/5
        [Route("api/Reserve/{type:int}/{username:maxlength(256)}")]
        [ResponseType(typeof(List<ReservationsView>))]
        public async Task<IHttpActionResult> GetReserve(int type, string username)
        {
            Expression<Func<ReservationsView, bool>> where;

            switch (type)
            {
                case 1: where = x => x.Date <= getTime && x.Accepted;
                    break;
                case 2: where = x => x.UserName == username;
                    break;
                case 3: where = x => x.Rejected;
                    break;
                default: where = x => true;
                    break;
            }

            var reservations = await db.ReservationsViews.Where(where).OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.StartTime).ToListAsync();

            return Ok(reservations);
        }

        // GET: api/Reserve/5
        [Route("api/ReserveForAdmin/{type:int}/{username:maxlength(256)}/{id:int}")]
        public async Task<IHttpActionResult> GetReserve(int type, string username, int id)
        {
            Expression<Func<ReservationsView, bool>> where;

            switch (type)
            {
                case 1: where = x => DbFunctions.TruncateTime(x.StartDateTime) >= getTime.Date && x.SalonID == id;
                    break;
                default: where = x => x.SalonID == id;
                    break;

            }

            var reservations = await db.ReservationsViews.Where(where).OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.StartTime).Select(x => new
                {
                    OrderID = x.OrderID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    Date = x.Date,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    ServicesCount = x.ServicesCount,
                    TotalPrice = x.TotalPrice,
                    Accepted = x.Accepted,
                    Rejected = x.Rejected,
                    RejectedByUser = x.RejectedByUser,
                    DidntCome = x.DidntCome,
                    Finished = x.Finished,
                    Sex = x.Sex,
                    UniqueID = x.UniqueID,
                    MoreInfo = x.MoreInfo
                }).ToListAsync();

            return Ok(reservations);
        }

        // GET: api/Reserve/5
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/ReserveForSchedule/{id:int}/")]
        public async Task<IHttpActionResult> GetReserveForSch(int id)
        {
            var reservations = await db.ReservationsViews.Where(x => x.SalonID == id && !x.IsDeleted)
                .OrderBy(x => x.StartDateTime).ThenBy(x => x.EndDateTime)
                .Select(x => new
                {
                    OrderID = x.OrderID,
                    incrementsBadgeTotal = true,
                    title = x.FirstName + " " + x.LastName,
                    startsAt = x.StartDateTime,
                    endsAt = x.EndDateTime,
                    UniqueID = x.UniqueID,
                    type = ((x.Accepted && !x.Finished && !x.DidntCome) ? "important" : (x.Rejected && !x.Finished && !x.DidntCome) ?
                    "warning" : (x.Accepted && x.Finished && !x.DidntCome) ? "success" : "info"),
                    deletable = x.IsFromAdmin,
                    // The type of the event (determines its color). Can be important, warning, info, inverse, success or special
                }).ToListAsync(); //TODO: да се добавят филтри

            return Ok(reservations);
        }

        // GET: api/Reserve/5
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/ReserveForScheduleById/{id:guid}/")]
        public async Task<IHttpActionResult> GetReserveForSchById(Guid id)
        {
            var reservations = await db.Orders.Where(x => x.UniqueID == id)
                .Select(x => new
                {
                    OrderID = x.OrderID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UniqueID = x.UniqueID,
                    StartDateTime = x.StartDateTime,
                    Services = x.OrderDetails.Select(y => y.ServiceID),
                    EmployeeID = x.OrderDetails.Select(y => y.EmployeeID).FirstOrDefault(),
                    SalonID = x.SalonID
                }).FirstOrDefaultAsync();

            return Ok(reservations);
        }

        // GET: api/Reserve/GetServicesById/5
        [Route("api/Reserve/GetServicesById/{id:int}/{type:max(5)}")]
        public async Task<IHttpActionResult> GetServicesById(int id, int type)
        {
            if (type == 1)
            {
                var services = await db.OrderDetails.Where(x => x.OrderID == id).Select(x => new
                {
                    ServiceID = x.ServiceID,
                    Price = x.Price,
                    ServiceName = x.Service.ServiceName
                }).ToListAsync();

                return Ok(services);
            }
            else
            {
                var services = await db.OrderDetails.Where(x => x.OrderID == id).Select(x => new
                {
                    ServiceID = x.ServiceID,
                    Price = x.Price,
                    ServiceName = x.Service.ServiceName,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    DontCare = x.DontCare
                }).ToListAsync();

                return Ok(services);
            }
        }

        // GET: api/Reserve/GetReservationCount/5
        [Route("api/Reserve/GetReservationCount/{username:maxlength(256)}")]
        [ResponseType(typeof(int[]))]
        public async Task<IHttpActionResult> GetReservationCount(string username, [FromUri] bool isAdmin)
        {
            int[] count = new int[2];
            if (!isAdmin)
            {
                count[0] = await db.Orders.Where(x => x.UserName == username).CountAsync();
                count[1] = await db.Favourites.Where(x => x.CreateBy == username).CountAsync();
            }
            else
            {
                count[0] = await db.Orders.Where(x => x.Salon.CreateBy == username).CountAsync();
            }
            return Ok(count);
        }

        #region Админ

        // GET: api/Reserve/GetReservationCount/5
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Reserve/GetReservationCountForAdmin/{username:maxlength(256)}")]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> GetReservationCountAdmin(string username)
        {
            int count = 0;
            count = await db.Orders.Where(x => x.Salon.CreateBy == username && x.Date == getTime.Date).CountAsync();

            return Ok(count);
        }

        // GET: api/Reserve/GetForAdminTable/памаретри
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Reserve/GetForAdminTable/")]
        public async Task<IHttpActionResult> GetReserveAdminTable([FromUri]SearchParams parameters)
        {
            var reservations = db.ReservationsViews.Where(x => x.CreateBy.ToLower() == parameters.UserName.ToLower() && x.SalonID == parameters.SalonID
                ).OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.StartTime).Select(x => new
                {
                    OrderID = x.OrderID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    Date = x.Date,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    ServicesCount = x.ServicesCount,
                    TotalPrice = x.TotalPrice,
                    Accepted = x.Accepted,
                    Rejected = x.Rejected,
                    RejectedByUser = x.RejectedByUser,
                    DidntCome = x.DidntCome,
                    Finished = x.Finished,
                    Sex = x.Sex,
                    UniqueID = x.UniqueID,
                    MoreInfo = x.MoreInfo,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            if (parameters.From.HasValue)
            {
                reservations = reservations.Where(x => DbFunctions.TruncateTime(x.StartDateTime) >= DbFunctions.TruncateTime(parameters.From.Value));
            }
            else
            {
                reservations = reservations.Where(x => DbFunctions.TruncateTime(x.StartDateTime) >= getTime);
            }

            if (parameters.To.HasValue)
            {
                reservations = reservations.Where(x => DbFunctions.TruncateTime(x.StartDateTime) <= DbFunctions.TruncateTime(parameters.To.Value));
            }
            else
            {
                reservations = reservations.Where(x => DbFunctions.TruncateTime(x.StartDateTime) <= getTime.Date.AddMonths(1));
            }

            if (!String.IsNullOrEmpty(parameters.Status))
            {
                switch (parameters.Status)
                {
                    case "Waiting": reservations = reservations.Where(x => !x.Accepted && !x.Rejected && !x.RejectedByUser);
                        break;
                    case "Accepted": reservations = reservations.Where(x => x.Accepted && !x.Finished && !x.DidntCome);
                        break;
                    case "Rejected": reservations = reservations.Where(x => x.Rejected);
                        break;
                    case "RejectedByUser": reservations = reservations.Where(x => x.RejectedByUser);
                        break;
                    case "DidntCome": reservations = reservations.Where(x => x.Accepted && x.DidntCome);
                        break;
                    case "Finished": reservations = reservations.Where(x => x.Accepted && x.Finished);
                        break;
                    default:
                        break;
                }
            }

            if (!String.IsNullOrWhiteSpace(parameters.SearchText))
            {
                reservations = reservations.Where(x => x.FirstName.Contains(parameters.SearchText)
                    || x.LastName.Contains(parameters.SearchText) || (x.FirstName + " " + x.LastName).Contains(parameters.SearchText));
            }

            var reservationsForReturn = await reservations.ToListAsync();

            return Ok(reservationsForReturn);
        }

        // GET: api/Reserve/GetForAdminTable/памаретри
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Reserve/GetForAdminModal/{id:guid}")]
        public async Task<IHttpActionResult> GetForAdminModal(Guid id)
        {
            var reservations = await db.ReservationsViews.Where(x => x.UniqueID == id).OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.StartTime).Select(x => new
                {
                    OrderID = x.OrderID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    Date = x.Date,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    ServicesCount = x.ServicesCount,
                    TotalPrice = x.TotalPrice,
                    Accepted = x.Accepted,
                    Rejected = x.Rejected,
                    RejectedByUser = x.RejectedByUser,
                    DidntCome = x.DidntCome,
                    Finished = x.Finished,
                    Sex = x.Sex,
                    UniqueID = x.UniqueID,
                    MoreInfo = x.MoreInfo,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    OrderDetails = db.OrderDetails.Where(y => y.OrderID == x.OrderID).Select(y => new
                    {
                        ServiceID = y.ServiceID,
                        Price = y.Price,
                        ServiceName = y.Service.ServiceName,
                        EmployeeName = y.Employee.FirstName + " " + y.Employee.LastName,
                        DontCare = y.DontCare,
                        ServiceType = y.Service.ServiceType.TypeName,
                        Time = y.Service.Time
                    })
                }).FirstOrDefaultAsync();



            return Ok(reservations);
        }

        #endregion

        // POST: api/Reserve
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostOrders(OrderInsert order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DateTime iKnowThisIsUtc = order.Date;
            DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind(
                iKnowThisIsUtc,
                DateTimeKind.Utc);
            var localVersion = TimeZoneInfo.ConvertTimeFromUtc(runtimeKnowsThisIsUtc, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));//offset.ToOffset(new TimeSpan(3, 0, 0));

            Order newO = new Order()
            {
                FirstName = order.FirstName,
                LastName = order.LastName,
                StartTime = order.StartTime,
                EndTime = order.EndTime,
                Phone = order.Phone,
                SalonID = order.SalonID,
                Sex = order.Sex,
                UserName = order.UserName,
                OrderDetails = order.Services,
                CreateOn = getTime,
                Date = localVersion,
                StartDateTime = localVersion.Add(order.StartTime),
                EndDateTime = localVersion.Add(order.EndTime),
                MoreInfo = order.MoreInfo,
                UniqueID = Guid.NewGuid(),
            };

            db.Orders.Add(newO);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = newO.OrderID }, newO);
        }

        // POST: api/Reserve/ForAdmin
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Reserve/PostEvent", Name = "PostEvent")]
        [HttpPost]
        public async Task<IHttpActionResult> PostOrders(OrderInsertEvent order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order newO = new Order();

            newO.FirstName = order.FirstName;
            newO.LastName = order.LastName;
            newO.SalonID = order.SalonID;
            newO.UserName = order.UserName;
            newO.CreateOn = getTime;
            newO.UniqueID = Guid.NewGuid();
            newO.IsFromAdmin = true;

            var services = db.Services.Where(x => order.Services.Contains(x.ServiceID));
            List<OrderDetail> details = new List<OrderDetail>();
            long serviceTicks = 0;

            foreach (var item in services)
            {
                details.Add(new OrderDetail
                {
                    DontCare = false,
                    EmployeeID = order.EmployeeID,
                    Price = item.Price,
                    ServiceID = item.ServiceID
                });

                serviceTicks += item.Time.Ticks;
            }

            newO.OrderDetails = details;

            DateTime iKnowThisIsUtc = order.StartDateTime;
            DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind(
                iKnowThisIsUtc,
                DateTimeKind.Utc);
            var localVersion = TimeZoneInfo.ConvertTimeFromUtc(runtimeKnowsThisIsUtc, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));//offset.ToOffset(new TimeSpan(3, 0, 0));


            newO.Date = localVersion.Date;
            newO.StartDateTime = localVersion;

            DateTime endDt = localVersion;
            TimeSpan startTime = new TimeSpan(), endTime = new TimeSpan();

            var servicesTotalTime = serviceTicks;

            startTime = new TimeSpan(endDt.Hour, endDt.Minute, 0);
            endTime = new TimeSpan(startTime.Ticks + servicesTotalTime);

            endDt = endDt.AddTicks(servicesTotalTime);

            newO.EndDateTime = endDt;
            newO.StartTime = startTime;
            newO.EndTime = endTime;


            db.Orders.Add(newO);
            await db.SaveChangesAsync();

            return Ok(newO.UniqueID);
        }

        // PUT: api/Orders/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Reserve/PutEvent/{id:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> PostOrders(Guid id, OrderInsertEvent order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.UniqueID)
            {
                return BadRequest();
            }

            Order orderFromdb = await db.Orders.Where(x => x.UniqueID == order.UniqueID).FirstOrDefaultAsync();

            db.OrderDetails.RemoveRange(orderFromdb.OrderDetails);

            orderFromdb.FirstName = order.FirstName;
            orderFromdb.LastName = order.LastName;

            var services = db.Services.Where(x => order.Services.Contains(x.ServiceID));
            long serviceTicks = 0;

            foreach (var item in services)
            {
                db.OrderDetails.Add(new OrderDetail
                {
                    DontCare = false,
                    EmployeeID = order.EmployeeID,
                    Price = item.Price,
                    ServiceID = item.ServiceID,
                    OrderID = orderFromdb.OrderID
                });

                serviceTicks += item.Time.Ticks;
            }

            DateTime iKnowThisIsUtc = order.StartDateTime;
            DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind(
                iKnowThisIsUtc,
                DateTimeKind.Utc);
            var localVersion = TimeZoneInfo.ConvertTimeFromUtc(runtimeKnowsThisIsUtc, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));//offset.ToOffset(new TimeSpan(3, 0, 0));


            orderFromdb.Date = localVersion.Date;
            orderFromdb.StartDateTime = localVersion;

            DateTime endDt = localVersion;
            TimeSpan startTime = new TimeSpan(), endTime = new TimeSpan();

            var servicesTotalTime = serviceTicks;

            startTime = new TimeSpan(endDt.Hour, endDt.Minute, 0);
            endTime = new TimeSpan(startTime.Ticks + servicesTotalTime);

            endDt = endDt.AddTicks(servicesTotalTime);

            orderFromdb.EndDateTime = endDt;
            orderFromdb.StartTime = startTime;
            orderFromdb.EndTime = endTime;

            db.Entry(orderFromdb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Orders/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.UniqueID)
            {
                return BadRequest();
            }

            Order orderFromdb = await db.Orders.FindAsync(order.OrderID);

            if (order.RejectedByUser)
            {
                if (!orderFromdb.Accepted && !orderFromdb.Rejected && !orderFromdb.DidntCome && !orderFromdb.Finished && !orderFromdb.RejectedByUser)
                {
                    orderFromdb.RejectedByUser = true;
                    orderFromdb.RejectedByUserTime = getTime;
                    orderFromdb.RejectedByUserReason = order.RejectedByUserReason;
                    orderFromdb.RejectedBy = order.RejectedBy;

                    db.Entry(orderFromdb).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }

            if (order.Rejected && !order.Finished && !order.DidntCome)
            {
                if (!orderFromdb.Accepted && !orderFromdb.Rejected && !orderFromdb.DidntCome && !orderFromdb.Finished && !orderFromdb.RejectedByUser)
                {
                    orderFromdb.Rejected = true;
                    orderFromdb.RejectedTime = getTime;

                    db.Entry(orderFromdb).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }

            if (order.Accepted && !order.Finished && !order.DidntCome)
            {
                if (!orderFromdb.Accepted && !orderFromdb.Rejected && !orderFromdb.DidntCome && !orderFromdb.Finished && !orderFromdb.RejectedByUser)
                {
                    orderFromdb.Accepted = true;
                    orderFromdb.AcceptedTime = getTime;

                    db.Entry(orderFromdb).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }

            if (order.Accepted && order.Finished && !order.DidntCome)
            {
                if (orderFromdb.Accepted && !orderFromdb.Rejected && !orderFromdb.DidntCome && !orderFromdb.Finished && !orderFromdb.RejectedByUser)
                {
                    orderFromdb.Finished = true;

                    db.Entry(orderFromdb).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }

            if (order.Accepted && !order.Finished && order.DidntCome)
            {
                if (orderFromdb.Accepted && !orderFromdb.Rejected && !orderFromdb.DidntCome && !orderFromdb.Finished && !orderFromdb.RejectedByUser)
                {
                    orderFromdb.DidntCome = true;

                    db.Entry(orderFromdb).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Orders/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Reserve/ChangeTime/{id:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangeTime(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.UniqueID)
            {
                return BadRequest();
            }

            Order orderFromdb = await db.Orders.FindAsync(order.OrderID);

            orderFromdb.Date = order.Date.Date;
            orderFromdb.StartDateTime = order.StartDateTime.ToLocalTime();

            DateTime endDt = order.StartDateTime.ToLocalTime();
            TimeSpan startTime = new TimeSpan(), endTime = new TimeSpan();

            var servicesTotalTime = orderFromdb.OrderDetails.Sum(x => x.Service.Time.Ticks);

            startTime = new TimeSpan(endDt.Hour, endDt.Minute, 0);
            endTime = new TimeSpan(startTime.Ticks + servicesTotalTime);

            endDt = endDt.AddTicks(servicesTotalTime);

            orderFromdb.EndDateTime = endDt;
            orderFromdb.StartTime = startTime;
            orderFromdb.EndTime = endTime;

            db.Entry(orderFromdb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                order.Date = orderFromdb.Date;
                order.StartDateTime = orderFromdb.StartDateTime;
                order.EndDateTime = orderFromdb.EndDateTime;
                order.StartTime = orderFromdb.StartTime;
                order.EndTime = orderFromdb.EndTime;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(order);
        }

        // DELETE: api/Orders/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Reserve/{id:guid}/{username:maxlength(100)}")]
        public async Task<IHttpActionResult> DeleteOrder(Guid id, string username)
        {
            Order order = await db.Orders.Where(x => x.UniqueID == id).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            order.IsDeleted = true;
            order.DeletedBy = username;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
