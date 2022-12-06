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
using SRLCore.Model;

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : DefaultController
    {

        public PlanController(IDistributedCache distributedCache, ILogger<PlanController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن طرح")]
        public async Task<IActionResult> AddPlan([FromBody] AddPlanRequest request)
        { 

            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation();

            var entity = request.ToEntity();

            await Db.AddSave(entity);

            return response.ToResponse(entity,TarhApi.Models.SelectableField.CommonPropertySelector);
        }

        [HttpPost("search")]
        [DisplayName("جستجوی طرح")]
        public async Task<IActionResult> SearchPlan([FromBody] SearchPlanRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetPlans(request).Paging(response, request.page_start, request.page_size)
                .Include(x => x.assessor_position)
                .Include(x => x.applicant_company).ThenInclude(x => x.city).ThenInclude(x => x.province).
                Include(x => x.evaluation_unit)
                .Include(x => x.expert).ThenInclude(x => x.user)
                .Include(x => x.knowledge_based_company_type).Include(x => x.lifecycle).Include(x => x.technology)
                .Include(x => x.type).Include(x => x.events).ThenInclude(x => x.level_event).ThenInclude(x => x.level)
                 .ToListAsync();
            return response.ToResponse(query, TarhApi.Models.SelectableField.PlantSelector);
        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده طرح")]
        public async Task<IActionResult> GetPlan(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetPlan(new Plan { id = id });
            existingEntity.ThrowIfNotExist();
            Db.Entry(existingEntity).Reference(x => x.applicant_company).Query().Include(y => y.city).ThenInclude(x => x.province).Load();
            Db.Entry(existingEntity).Reference(x => x.assessor_position).Load();
            Db.Entry(existingEntity).Reference(x => x.evaluation_unit).Load();
            Db.Entry(existingEntity).Reference(x => x.expert).Query().Include(x => x.user).Load();
            Db.Entry(existingEntity).Reference(x => x.knowledge_based_company_type).Load();
            Db.Entry(existingEntity).Reference(x => x.lifecycle).Load();
            Db.Entry(existingEntity).Reference(x => x.technology).Load();
            Db.Entry(existingEntity).Reference(x => x.type).Load();
            Db.Entry(existingEntity).Collection(x => x.events).Query().Include(x => x.level_event).ThenInclude(x => x.level).Load();

            return response.ToResponse(existingEntity, TarhApi.Models.SelectableField.PlantSelector);
        }

        [DisplayName("ویرایش طرح")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditPlan([FromBody] AddPlanRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation();

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetPlan(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.assessor_position_id = entity.assessor_position_id;
            existingEntity.applicant_id = entity.applicant_id;
            existingEntity.estimate_riali = entity.estimate_riali;
            existingEntity.evaluation_unit_id = entity.evaluation_unit_id;
            existingEntity.expert_id = entity.expert_id;
            existingEntity.knowledge_based_company_type_id = entity.knowledge_based_company_type_id;
            existingEntity.lifecycle_id = entity.lifecycle_id;
            existingEntity.referral_pdate = entity.referral_pdate;
            existingEntity.subject = entity.subject;
            existingEntity.technology_id = entity.technology_id;
            existingEntity.type_id = entity.type_id;


            await Db.UpdateSave();

            return response.ToResponse(existingEntity, TarhApi.Models.SelectableField.CommonPropertySelector);
        }

        [DisplayName("حذف طرح")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlan(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetPlan(new Plan { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }

        [HttpPost("dashboard")]
        [DisplayName("مشاهده داشبورد طرح")]
        public async Task<IActionResult> ViewPlanDashboard()
        {

            SingleResponse<object> response = new SingleResponse<object>();
            decimal plan_count_all = await Db.GetPlans().CountAsync();
            IEnumerable<BaseInfo> type_share = null;
            Task type_share_task = Task.Run(async () =>
            {
                var plans = await Db.GetPlans().ToListAsync();
                var type_share_g = plans.GroupBy(x => x.type_id);
                var type_share_sel = type_share_g.Select(x =>
                {
                    int count = x.Count();
                    return new { type_id = x.Key, count, type_share = (count / plan_count_all).ToString("0.##") };
                });
              type_share= type_share_sel.Join(Db.GetBaseInfos(),
      plan => plan.type_id,
      type => type.id,
      (plan, type) => new BaseInfo { id = plan.type_id, title = type.title, plan_share = plan.type_share, plan_count = plan.count });
            });

            IEnumerable<Level> plan_level = null;
            Task plan_level_task = Task.Run(() =>
            {
                plan_level = Db.GetPlans().IncludeLastLevel(Db)
                .GroupBy(x => new { x.last_level.id, x.last_level.title })
                .AsEnumerable()
                .Select(x =>
                {
                    int count = x.Count();
                    return new Level
                    {
                        id = x.Key.id,
                        title = x.Key.title,
                        last_plan_count = count,
                        last_plan_share = (count / plan_count_all).ToString("0.##")
                    };
                });

            });


            Task.WaitAll(type_share_task, plan_level_task);

            return response.ToResponse(new { type_share, plan_level });
        }

        [HttpPost("report/summary")]
        [DisplayName("مشاهده خلاصه گزارش طرح")]
        public async Task<IActionResult> TakePlanReportSummary([FromBody] PagedRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await  Db.GetPlans().Paging(response, request.page_start, request.page_size)
               .IncludeLastLevelEvent(Db)
                .Include(x => x.type).Include(x => x.applicant_company)
                 .ToListAsync();
            return response.ToResponse(query.Select(x => new
            {
                x.subject,
                x.id,
                x.type_id,
                x.type_title,
                x.applicant_id,
                x.applicant_name,
                x.referral_pdate,
                x.last_level,
                x.last_event
            }));
        }

        [HttpPost("report/forgotten")]
        [DisplayName("مشاهده گزارش طرح در دست فراموشی")]
        public async Task<IActionResult> TakePlanReportForgotten([FromBody] PagedRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetPlans().Paging(response, request.page_start, request.page_size)
               .IncludeLastLevelEvent(Db)
                 .ToListAsync();
            return response.ToResponse(query.Select(x => new
            {
                x.subject,
                x.id,
                x.last_level,
                x.last_event
            }));
        }
    }
}