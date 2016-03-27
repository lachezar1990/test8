using System.Collections.Generic;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Companies;
using ZapaziMi.DAL.Entities.ZapaziMiDb;

namespace ZapaziMi.DAL.Companies
{
    public interface ICompaniesDAL
    {
        Task<List<GetCompanyEntity>> GetCompanies();
        Task<GetCompanyByUsername> GetCompanyByUsername(string username);
        Task<bool> UpdateCompany(int id, Company company);
        Task<Company> InsertCompany(Company company);
        Task<bool> DeleteCompany(int id);
    }
}
