//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationppp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int OrderID { get; set; }
        public int SalonID { get; set; }
        public System.Guid UniqueID { get; set; }
        public string UserName { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime CreateOn { get; set; }
        public bool Accepted { get; set; }
        public Nullable<System.DateTime> AcceptedTime { get; set; }
        public bool Rejected { get; set; }
        public Nullable<System.DateTime> RejectedTime { get; set; }
        public bool Changed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Nullable<byte> Sex { get; set; }
        public bool Finished { get; set; }
        public bool DidntCome { get; set; }
        public bool RejectedByUser { get; set; }
        public Nullable<System.DateTime> RejectedByUserTime { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedByUserReason { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public string MoreInfo { get; set; }
        public bool IsFromAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
    
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Salon Salon { get; set; }
    }
}
