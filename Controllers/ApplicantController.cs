using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarhApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Microsoft.Build.Execution;
using System.ComponentModel;
using SRL;
using System.Net.Http;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using System.Text;
using TarhApi.Services;
using TarhApi.Middleware;

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : DefaultController
    {

        public ApplicantController(IDistributedCache distributedCache, ILogger<ApplicantController> logger, TarhDb dbContext, UserService userService) :
            base(distributedCache, logger, dbContext, userService)
        {
           

        }
        [HttpPost("add")]
        [DisplayName("افزودن متقاضی")]
        public async Task<IActionResult> AddApplicant([FromBody] AddApplicantRequest request)
        {
            return await base.Add<AddApplicantRequest, Applicant>(request);
        }

        [HttpPost("search")]
        [DisplayName("جستجوی متقاضی")]
        public async Task<IActionResult> SearchApplicant([FromBody] SearchApplicantRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetApplicants(request).Paging(response, request.page_start, request.page_size)
                .Include(x => x.city).Include(x => x.type)
                 .ToListAsync();

            return response.ToResponse(query, SelectableField.ApplicantSelector);
        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده متقاضی")]
        public async Task<IActionResult> GetApplicant(long id)
        {

            var existingEntity = await Db.GetApplicant(new Applicant { id = id });
            Db.Entry(existingEntity).Reference(x => x.city).Load();
            Db.Entry(existingEntity).Reference(x => x.type).Load();
            return base.Get(existingEntity, SelectableField.ApplicantSelector);
        }

        [DisplayName("ویرایش متقاضی")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditApplicant([FromBody] AddApplicantRequest request)
        {
            var entity = request.ToEntity2<Applicant>(request.id);

            var existingEntity = await Db.GetApplicant(entity);

            existingEntity.address = entity.address;
            existingEntity.name = entity.name;
            existingEntity.phone = entity.phone;
            existingEntity.related_category_id = entity.related_category_id;
            existingEntity.representative = entity.representative;
            existingEntity.type_id = entity.type_id;
            existingEntity.ceo = entity.ceo;
            existingEntity.city_id = entity.city_id;
            return await base.Edit(request, existingEntity);
        }

        [DisplayName("حذف متقاضی")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteApplicant(long id)
        {
            var existingEntity = await Db.GetApplicant(new Applicant { id = id });
            return await base.Delete<Applicant>(existingEntity);
        }
    }
}