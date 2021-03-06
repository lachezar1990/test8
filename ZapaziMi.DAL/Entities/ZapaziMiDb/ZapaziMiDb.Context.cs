﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Diplomna_newEntities : DbContext
    {
        public Diplomna_newEntities()
            : base("name=Diplomna_newEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Neighbourhood> Neighbourhoods { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<SalonImage> SalonImages { get; set; }
        public virtual DbSet<Salon> Salons { get; set; }
        public virtual DbSet<SalonSchedule> SalonSchedules { get; set; }
        public virtual DbSet<ServiceImage> ServiceImages { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<EmplWithPosition> EmplWithPositions { get; set; }
        public virtual DbSet<ReservationsView> ReservationsViews { get; set; }
        public virtual DbSet<SalonDetailsInfo> SalonDetailsInfoes { get; set; }
        public virtual DbSet<SalonsMainScreenMobile> SalonsMainScreenMobiles { get; set; }
        public virtual DbSet<ServicesForDetailsPage> ServicesForDetailsPages { get; set; }
    }
}
