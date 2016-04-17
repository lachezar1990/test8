using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Companies;
using ZapaziMi.DAL.Entities.Companies;
using ZapaziMi.DAL.Entities.ZapaziMiDb;
using ZapaziMi.WebAPI.Services.Models;

namespace ZapaziMi.WebAPI.Services.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private ICompaniesDAL _companiesDal;

        public CompaniesService(ICompaniesDAL companiesDal)
        {
            _companiesDal = companiesDal;
        }

        public async Task<List<GetCompanyEntity>> GetCompanies()
        {
            return await _companiesDal.GetCompanies();
        }

        public async Task<GetCompanyByUsername> GetCompanyByUsername(string username)
        {
            return await _companiesDal.GetCompanyByUsername(username);
        }

        public async Task<ResponseModel> UpdateCompany(int id, Company company)
        {
            ResponseModel result = new ResponseModel();
            if (id != company.CompanyID)
            {
                result.Errors.Add(new Error()
                {
                    ErrorType = Models.Enums.ErrorTypes.Validation,
                    ShortMessage = "Номерата на компанията на са еднакви!"
                });
                return result;
            }

            try
            {
                bool success = await _companiesDal.UpdateCompany(id, company);
                if (!success)
                {
                    result.Errors.Add(new Error()
                    {
                        ErrorType = Models.Enums.ErrorTypes.NotFound,
                        ShortMessage = "Фирмата не беше намерена!"
                    });
                    return result;
                }

                result.Messages.Add("Данните за фирмата бяха обновени!");
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Error()
                {
                    ErrorType = Models.Enums.ErrorTypes.Exception,
                    ShortMessage = "Възникна проблем със запазването на данните!",
                    LongMessage = ex.Message.ToString()
                });
                return result;
            }

            return result;
        }

        public async Task<Tuple<int, Company>> InsertCompany(Company company)
        {
            company.AddedOn = DateTime.Now;
            company.IsDeleted = false;

            company = await _companiesDal.InsertCompany(company);

            return new Tuple<int, Company>(company.CompanyID, company);
        }

        public async Task<ResponseModel<Company>> DeleteCompany(int id)
        {
            ResponseModel<Company> result = new ResponseModel<Company>();
            bool success = await _companiesDal.DeleteCompany(id);
            if (!success)
            {
                result.Errors.Add(new Error()
                {
                    ErrorType = Models.Enums.ErrorTypes.NotFound,
                    ShortMessage = "Фирмата не беше намерена!"
                });
                return result;
            }

            result.Messages.Add("Фирмата беше успешно изтрита");
            return result;
        }
    }
}
