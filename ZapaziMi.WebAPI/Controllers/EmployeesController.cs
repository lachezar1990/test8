using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationppp.Models;

namespace WebApplicationppp.Controllers
{
    public class EmployeesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        //// GET: api/Employees
        //public IQueryable<Employee> GetEmployees()
        //{
        //    return db.Employees;
        //}

        // GET: api/Employees/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(EmployeesDTO))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            EmployeesDTO empl = new EmployeesDTO();

            var employee = await db.Employees.Where(x => x.Position.SalonID == id && !x.Deleted).Select(x => new EmplForListDTO
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                PositionID = x.PositionID,
                PositionName = x.Position.PositionName,
                ImageUrl = x.ImageUrl,
                EmplSchedule = db.EmployeeSchedules.Where(z => z.EmployeeID == x.EmployeeID).Select(y => new
                {
                    Date = y.Date,
                    DayOfWeek = y.DayOfWeek,
                    EmployeeID = y.EmployeeID,
                    EndTime = y.EndTime,
                    Holiday = y.Holiday,
                    ScheduleID = y.ScheduleID,
                    StartTime = y.StartTime
                })
            }).ToListAsync();

            empl.Employees = employee;
            empl.PositionsCount = await db.Positions.CountAsync(x => !x.Deleted && x.SalonID == id);

            return Ok(empl);
        }

        // GET: api/Employees/GetById/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(EmplForListDTO))]
        [Route("api/Employees/GetById/{id:int}")]
        public async Task<IHttpActionResult> GetEmployeeById(int id)
        {
            var employee = await db.Employees.Where(x => x.EmployeeID == id && !x.Deleted).Select(x => new EmplForListDTO
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                PositionID = x.PositionID,
                PositionName = x.Position.PositionName,
                ImageUrl = x.ImageUrl

            }).FirstOrDefaultAsync();

            return Ok(employee);
        }

        // GET: api/Employees/GetForEvent/5
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Employees/GetForEvent/{id:int}")]
        public async Task<IHttpActionResult> GetEmployeeEvent(int id)
        {
            var employee = await db.Employees.Where(x => x.Position.SalonID == id && !x.Deleted).Select(x => new
            {
                EmployeeID = x.EmployeeID,
                FullName = x.FirstName + " " + x.LastName,
                PositionName = x.Position.PositionName,

            }).ToListAsync();

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            Employee emplFromDb = await db.Employees.FindAsync(id);

            emplFromDb.FirstName = employee.FirstName;
            emplFromDb.MiddleName = employee.MiddleName;
            emplFromDb.LastName = employee.LastName;
            emplFromDb.PositionID = employee.PositionID;

            db.Entry(emplFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // PUT: api/Employees/DeleteImage/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DelEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            Employee emplFromDb = await db.Employees.FindAsync(id);

            emplFromDb.ImageUrl = null;

            db.Entry(emplFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // PUT: api/Employees/DeleteImage/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Employees/ChangeWorkingTime/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> DelEmployee(int id, List<EmployeeSchedule> employeeSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<EmployeeSchedule> emplScheduleFromDb = await db.EmployeeSchedules.Where(x => x.EmployeeID == id).ToListAsync();

            if (emplScheduleFromDb.Count == 7)
            {
                int i = 0;
                foreach (var day in emplScheduleFromDb)
                {
                    day.Holiday = employeeSchedule[i].Holiday;
                    day.StartTime = employeeSchedule[i].StartTime;
                    day.EndTime = employeeSchedule[i].EndTime;
                    i++;
                }
            }
            else
            {
                foreach (var day in employeeSchedule)
                {
                    db.EmployeeSchedules.Add(new EmployeeSchedule
                    {
                        DayOfWeek = day.DayOfWeek,
                        EndTime = day.EndTime,
                        StartTime = day.StartTime,
                        Holiday = day.Holiday,
                        EmployeeID = id
                    });
                }


            }

            //db.Entry(emplScheduleFromDb).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employee.Deleted = false;
            employee.AddedOn = DateTime.Now;

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            for (byte i = 1; i <= 7; i++)
            {
                db.EmployeeSchedules.Add(new EmployeeSchedule
                {
                    DayOfWeek = i,
                    EmployeeID = employee.EmployeeID,
                    EndTime = new TimeSpan(18, 0, 0),
                    StartTime = new TimeSpan(10, 0, 0),
                    Holiday = false
                });
            }

            await db.SaveChangesAsync();

            var returnEmp = new EmplForListDTO()
            {
                EmployeeID = employee.EmployeeID,
                EmplSchedule = null,
                FirstName = employee.FirstName,
                ImageUrl = employee.ImageUrl,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                PositionID = employee.PositionID
            };

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, returnEmp);
        }

        // DELETE: api/Employees/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Employees/{id:int}/{username:maxlength(256)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEmployee(int id, string username)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Deleted = true;
            employee.DeletedBy = username;
            employee.DeletedDate = DateTime.Now;

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

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}