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
    
    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public int ServiceID { get; set; }
        public decimal Price { get; set; }
        public bool DontCare { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Order Order { get; set; }
        public virtual Service Service { get; set; }
    }
}
