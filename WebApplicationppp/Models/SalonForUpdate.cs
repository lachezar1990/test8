using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationppp.Models
{
    public class SalonForUpdate
    {
        public Nullable<int> SalonID { get; set; }
        public string SalonName { get; set; }
        public string Email { get; set; }
        public string SiteUrl { get; set; }
        public int CityID { get; set; }
        public Nullable<int> NeighbourhoodID { get; set; }
        public string SalonStreet { get; set; }
        public string SalonFlat { get; set; }
        public string SalonNumber { get; set; }
        public string SalonEntrance { get; set; }
        public string SalonApartment { get; set; }
        public string SalonPhones { get; set; }
        public Boolean VisibleForUsers { get; set; }
        public string SalonDescription { get; set; }
        public int CompanyID { get; set; }
        public string Username { get; set; }
        public SalonSchedule[] SalonSchedule { get; set; }
    }

    public class SalonImageForDelete
    {
        public int ImageID { get; set; }
        public string Username { get; set; }
        public Boolean ForDelete { get; set; }
    }

    public class EmplForListDTO
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string ImageUrl { get; set; }
        public IQueryable<dynamic> EmplSchedule { get; set; }
    }

    public class EmployeesDTO
    {
        public List<EmplForListDTO> Employees { get; set; }
        public int PositionsCount { get; set; }
    }

    public class PositionToAddDTO
    {
        public string PositionName { get; set; }
        public int SalonID { get; set; }
        public string UserName { get; set; }
    }

    public class PositionForDelete
    {
        public int PositionID { get; set; }
        public string UserName { get; set; }
        public Boolean IsForDelete { get; set; }
        public string PositionName { get; set; }
    }

    public class ServiceListDTO
    {
        public DateTime AddedOn { get; set; }
        public string Description { get; set; }
        public Boolean Kids { get; set; }
        public Boolean Men { get; set; }
        public int? OrderNumber { get; set; }
        public decimal Price { get; set; }
        public int SalonID { get; set; }
        public int ServiceID { get; set; }
        public string ImageUrl { get; set; }
        public string ServiceName { get; set; }
        public int ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public TimeSpan Time { get; set; }
        public Boolean Women { get; set; }
    }
}