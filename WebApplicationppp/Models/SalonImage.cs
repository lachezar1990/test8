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
    
    public partial class SalonImage
    {
        public int ImageID { get; set; }
        public int SalonID { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public System.DateTime AddedOn { get; set; }
        public string CreateBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public bool IsMain { get; set; }
    
        public virtual Salon Salon { get; set; }
    }
}
