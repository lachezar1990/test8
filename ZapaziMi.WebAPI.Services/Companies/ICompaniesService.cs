using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Companies;
using ZapaziMi.DAL.Entities.ZapaziMiDb;
using ZapaziMi.WebAPI.Services.Models;

namespace ZapaziMi.WebAPI.Services.Companies
{
    public interface ICompaniesService
    {
        Task<List<GetCompanyEntity>> GetCompanies();
        Task<GetCompanyByUsername> GetCompanyByUsername(string username);
        Task<ResponseModel> UpdateCompany(int id, Company company);
        Task<Tuple<int, Company>> InsertCompany(Company company);
        Task<ResponseModel<Company>> DeleteCompany(int id);
    }
}
