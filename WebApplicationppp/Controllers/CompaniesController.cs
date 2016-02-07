using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationppp.Models;

namespace WebApplicationppp.Controllers
{
    public class CompaniesController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Companies
        public IQueryable<Company> GetCompanies()
        {
            return db.Companies;
        }

        // GET: api/Companies/5
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Companies/{username:maxlength(150)}")]
        public async Task<IHttpActionResult> GetCompany(string username)
        {
            var company = await db.Companies.Where(x => x.CreateBy.ToUpper() == username.ToUpper() && !x.IsDeleted)
                .Select(x => new
                {
                    CompanyAddress = x.AddressText,
                    CityID = x.CityID,
                    CompanyID = x.CompanyID,
                    CompanyName = x.CompanyName,
                    CreateBy = x.CreateBy,
                    Email = x.Email,
                    CompanyPhones = x.Phones
                }).FirstOrDefaultAsync();

            //if (company == null)
            //{
            //    return NotFound();
            //}

            return Ok(company);
        }

        // PUT: api/Companies/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(void))]
        [Route("api/Companies/{id:int}")]
        public async Task<IHttpActionResult> PutCompany(int id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.CompanyID)
            {
                return BadRequest();
            }

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
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Companies
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            company.AddedOn = DateTime.Now;
            company.IsDeleted = false;

            db.Companies.Add(company);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = company.CompanyID }, company);
        }

        // DELETE: api/Companies/5
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> DeleteCompany(int id)
        {
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            await db.SaveChangesAsync();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.CompanyID == id) > 0;
        }
    }
}