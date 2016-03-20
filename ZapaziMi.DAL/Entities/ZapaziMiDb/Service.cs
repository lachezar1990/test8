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
    
    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ServiceImages = new HashSet<ServiceImage>();
        }
    
        public int ServiceID { get; set; }
        public int SalonID { get; set; }
        public int TypeID { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public System.TimeSpan Time { get; set; }
        public string Description { get; set; }
        public System.DateTime AddedOn { get; set; }
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public bool Men { get; set; }
        public bool Women { get; set; }
        public bool Kids { get; set; }
        public string ImageUrl { get; set; }
        public string AddedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Salon Salon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceImage> ServiceImages { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}
