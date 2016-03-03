using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationppp.Models;

namespace WebApplicationppp.Models
{
    public class OrderInsert
    {
        public int SalonID { get; set; }
        public string UserName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public byte Sex { get; set; }
        public List<OrderDetail> Services { get; set; }
        public DateTime Date { get; set; }
        public string MoreInfo { get; set; }
    }

    public class OrderInsertEvent
    {
        public int SalonID { get; set; }
        public string UserName { get; set; }
        public DateTime StartDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Services { get; set; }
        public int EmployeeID { get; set; }
        public Guid? UniqueID { get; set; }
    }
}