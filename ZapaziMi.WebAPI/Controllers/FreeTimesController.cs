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
    public class FreeTimesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/EmplWithPositions/5
        [ResponseType(typeof(ResponseServiceTimes))]
        public async Task<IHttpActionResult> GetEmplWithPosition([FromUri]TimesPar par)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!par.DontCare && !par.EmplId.HasValue) //ако го интересува служителя, но няма и служител
            {
                return BadRequest(ModelState);
            }

            ResponseServiceTimes ret = new ResponseServiceTimes();

            int dayOfW = ((int)par.DateTimeRes.DayOfWeek == 0) ? 7 : (int)par.DateTimeRes.DayOfWeek;

            if (par.DontCare)
            {
                SalonSchedule startEndTime = new SalonSchedule();
                startEndTime = await db.SalonSchedules.Where(
                    x => x.SalonID == par.SalonId && x.DayOfWeek == dayOfW).FirstOrDefaultAsync();

                if (startEndTime != null)
                {
                    if (!startEndTime.Holiday && startEndTime.StartTime.HasValue && startEndTime.EndTime.HasValue)
                    {
                        TimeSpan startTime = startEndTime.StartTime.Value;
                        TimeSpan endTime = startEndTime.StartTime.Value.Add(par.ServiceDur);
                        TimeSpan timeToAdd = TimeSpan.FromMinutes(15);
                        TimeSpan currentStartTime = startTime;
                        TimeSpan currentEndTime = endTime;
                        List<ServiceTime> times = new List<ServiceTime>();

                        do
                        {
                            ServiceTime tempTime = new ServiceTime();
                            tempTime.StartTime = currentStartTime;
                            tempTime.EndTime = currentEndTime;
                            times.Add(tempTime);

                            currentStartTime = currentStartTime.Add(timeToAdd);
                            currentEndTime = currentEndTime.Add(timeToAdd);
                        }
                        while (currentEndTime <= startEndTime.EndTime.Value);

                        ret.Result = "OK";
                        ret.times = times;
                        return Ok(ret);
                    }
                    else
                    {
                        ret.Result = "Holiday";
                        return Ok(ret);
                    }
                }
                else
                {
                    ret.Result = "No schedule";
                    return Ok(ret);
                }
            }
            else
            {
                EmployeeSchedule startEndTime = new EmployeeSchedule();
                startEndTime = await db.EmployeeSchedules.Where(
                    x => x.EmployeeID == par.EmplId && x.DayOfWeek == dayOfW).FirstOrDefaultAsync();

                if (startEndTime != null)
                {
                    if (!startEndTime.Holiday && startEndTime.StartTime.HasValue && startEndTime.EndTime.HasValue)
                    {
                        TimeSpan startTime = startEndTime.StartTime.Value;
                        TimeSpan endTime = startEndTime.StartTime.Value.Add(par.ServiceDur);
                        TimeSpan timeToAdd = TimeSpan.FromMinutes(15);
                        TimeSpan currentStartTime = startTime;
                        TimeSpan currentEndTime = endTime;
                        List<ServiceTime> times = new List<ServiceTime>();

                        do
                        {
                            ServiceTime tempTime = new ServiceTime();
                            tempTime.StartTime = currentStartTime;
                            tempTime.EndTime = currentEndTime;
                            times.Add(tempTime);

                            currentStartTime = currentStartTime.Add(timeToAdd);
                            currentEndTime = currentEndTime.Add(timeToAdd);
                        }
                        while (currentEndTime <= startEndTime.EndTime.Value);

                        ret.Result = "OK";
                        ret.times = times;
                        return Ok(ret);
                    }
                    else
                    {
                        ret.Result = "Holiday";
                        return Ok(ret);
                    }
                }
                else
                {
                    ret.Result = "No schedule";
                    return Ok(ret);
                }
            }
        }
    }
}
