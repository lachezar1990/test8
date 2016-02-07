using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationppp.Models
{
    public class SearchParams
    {
        public string UserName { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Status { get; set; }
        public string SearchText { get; set; }
        public int SalonID { get; set; }
    }
}