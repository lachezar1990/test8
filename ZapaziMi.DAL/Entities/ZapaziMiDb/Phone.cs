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
    
    public partial class Phone
    {
        public int PhoneID { get; set; }
        public int SalonID { get; set; }
        public string Phone1 { get; set; }
    
        public virtual Salon Salon { get; set; }
    }
}