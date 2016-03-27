using System;

namespace ZapaziMi.DAL.Entities.Companies
{
    public class GetCompanyEntity
    {
        public DateTime AddedOn { get; set; }
        public string AddressText { get; set; }
        public int CityID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CreateBy { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public string Phones { get; set; }
    }
}
