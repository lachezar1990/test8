//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZapaziMi.DAL.Entities.ZapaziMiDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        public int RatingID { get; set; }
        public byte Rating1 { get; set; }
        public int SalonID { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime AddedOn { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual Salon Salon { get; set; }
    }
}