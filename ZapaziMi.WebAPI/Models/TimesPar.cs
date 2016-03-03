using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationppp.Models
{
    public class TimesPar
    {
        public DateTime DateTimeRes { get; set; }
        public int SalonId { get; set; }
        public int? EmplId { get; set; }
        public bool DontCare { get; set; }
        public TimeSpan ServiceDur { get; set; }
    }

    public class ServiceTime
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class ResponseServiceTimes
    {
        public string Result { get; set; }
        public List<ServiceTime> times { get; set; }
    }

    public class DateTimesLocal
    {
        public DateTime DateTimeNowLocal()
        {
            DateTime dateTimeBg = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));
            return dateTimeBg;
        }
    }
}