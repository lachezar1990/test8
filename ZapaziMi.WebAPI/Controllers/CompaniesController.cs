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
using ZapaziMi.DAL.Entities.Companies;
using ZapaziMi.WebAPI.Services.Companies;
using ZapaziMi.WebAPI.Services.Models;

namespace WebApplicationppp.Controllers
{
    public class CompaniesController : BaseApiController
    {
        private ICompaniesService _companiesService;

        public CompaniesController(ICompaniesService companiesService)
        {
            _companiesService = companiesService;
        }

        // GET: api/Companies
        [HttpGet]
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Companies")]
        [ResponseType(typeof(List<GetCompanyEntity>))]
        public async Task<IHttpActionResult> GetCompanies()
        {
            return await GetMyResult(() => _companiesService.GetCompanies());
        }

        // GET: api/Companies/5
        [HttpGet]
        [Authorize(Roles = "SalonAdmin")]
        [Route("api/Companies/{username:maxlength(150)}")]
        [ResponseType(typeof(GetCompanyByUsername))]
        public async Task<IHttpActionResult> GetCompany(string username)
        {
            return await GetMyResult(() => _companiesService.GetCompanyByUsername(username));
        }

        // PUT: api/Companies/5
        [HttpPut]
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(ResponseModel))]
        [Route("api/Companies/{id:int}")]
        public async Task<IHttpActionResult> PutCompany(int id, ZapaziMi.DAL.Entities.ZapaziMiDb.Company company)
        {
            return await GetMyResultWithModelValidation(() => _companiesService.UpdateCompany(id, company));
        }

        // POST: api/Companies
        [HttpPost]
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> PostCompany(ZapaziMi.DAL.Entities.ZapaziMiDb.Company company)
        {
            return await GetMyInsertResultWithModelValidation(() => _companiesService.InsertCompany(company));
        }

        // DELETE: api/Companies/5
        [HttpDelete]
        [Authorize(Roles = "SalonAdmin")]
        [ResponseType(typeof(ResponseModel<Company>))]
        public async Task<IHttpActionResult> DeleteCompany(int id)
        {
            return await GetMyResult(() => _companiesService.DeleteCompany(id));
        }
    }
}