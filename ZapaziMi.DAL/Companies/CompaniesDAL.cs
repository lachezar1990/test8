using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapaziMi.DAL.Entities.Companies;
using ZapaziMi.DAL.Entities.ZapaziMiDb;

namespace ZapaziMi.DAL.Companies
{
    public class CompaniesDAL : ICompaniesDAL
    {
        private Diplomna_newEntities db;

        public async Task<List<GetCompanyEntity>> GetCompanies()
        {
            using (db = new Diplomna_newEntities())
            {
                List<GetCompanyEntity> companies = await db.Companies.Select(x => new GetCompanyEntity
                {
                    AddedOn = x.AddedOn,
                    AddressText = x.AddressText,
                    CityID = x.CityID,
                    CompanyID = x.CompanyID,
                    CompanyName = x.CompanyName,
                    CreateBy = x.CreateBy,
                    DeletedBy = x.DeletedBy,
                    DeletedDate = x.DeletedDate,
                    Email = x.Email,
                    IsDeleted = x.IsDeleted,
                    Phones = x.Phones
                }).ToListAsync();

                return companies;
            }
        }

        public async Task<GetCompanyByUsername> GetCompanyByUsername(string username)
        {
            using (db = new Diplomna_newEntities())
            {
                GetCompanyByUsername company = await db.Companies
                    .Where(x => x.CreateBy.ToUpper() == username.ToUpper() && !x.IsDeleted)
                    .Select(x => new GetCompanyByUsername
                    {
                        CompanyAddress = x.AddressText,
                        CityID = x.CityID,
                        CompanyID = x.CompanyID,
                        CompanyName = x.CompanyName,
                        CreateBy = x.CreateBy,
                        Email = x.Email,
                        CompanyPhones = x.Phones
                    }).FirstOrDefaultAsync();

                return company;
            }
        }

        public async Task<bool> UpdateCompany(int id, Company company)
        {
            using (db = new Diplomna_newEntities())
            {
                Company companyFromDb = await db.Companies.FindAsync(id);

                companyFromDb.AddressText = company.AddressText;
                companyFromDb.CityID = company.CityID;
                companyFromDb.CompanyName = company.CompanyName;
                companyFromDb.Email = company.Email;
                companyFromDb.Phones = company.Phones;

                db.Entry(companyFromDb).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await CompanyExists(id)))
                    {
                        return false; // does not exist
                    }
                    else
                    {
                        throw;
                    }
                }

                return true;
            }
        }

        public async Task<Company> InsertCompany(Company company)
        {
            using (db = new Diplomna_newEntities())
            {
                db.Companies.Add(company);
                await db.SaveChangesAsync();

                return company;
            }
        }

        public async Task<bool> DeleteCompany(int id)
        {
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return false;
            }

            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CompanyExists(int id)
        {
            return await db.Companies.CountAsync(e => e.CompanyID == id) > 0;
        }
    }
}
