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
using SRLCore.Model;
namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseInfoController : DefaultController
    {

        public BaseInfoController(IDistributedCache distributedCache, ILogger<BaseInfoController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن پایه")]
        public async Task<IActionResult> AddBaseInfo([FromBody] AddBaseInfoRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();

            await Db.AddSave(entity);

            return response.ToResponse(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                x.modifier_id,
                x.modify_date,
                x.title,
                x.kind,

            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی پایه")]
        public async Task<IActionResult> SearchBaseInfo([FromBody] SearchBaseInfoRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetBaseInfos(request).Paging(response, request.page_start, request.page_size) 
                 .ToListAsync(); 

            return response.ToResponse<BaseInfo>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.title,
                x.kind ,
                x.is_default
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده پایه")]
        public async Task<IActionResult> GetBaseInfo(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetBaseInfo(new BaseInfo { id = id });
            existingEntity.ThrowIfNotExist(); 

            return response.ToResponse<BaseInfo>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                x.title,
                x.kind,
                kind_id=(int)x.kind,
                x.is_default
            });
        }

        [DisplayName("ویرایش پایه")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditBaseInfo([FromBody] AddBaseInfoRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetBaseInfo(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.title = entity.title;
            existingEntity.kind = entity.kind;
            existingEntity.is_default = entity.is_default;
           
            await Db.UpdateSave(); 

            return response.ToResponse<BaseInfo>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title,
                x.kind
              
            });
        }

        [DisplayName("حذف پایه")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBaseInfo(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetBaseInfo(new BaseInfo { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }

        [HttpPost("search/kind")]
        [DisplayName("جستجوی انواع پایه")]
        public IActionResult SearchBaseKind()
        {
            PagedResponse<object> response = new PagedResponse<object>();
            List<object> base_kind_list = new List<object>();

            foreach (BaseKind base_ in (BaseKind[])Enum.GetValues(typeof(BaseKind)))
            {
                base_kind_list.Add(new { kind = (int)base_ ,
                    title =SRL.ClassManagement.GetEnumDescription<BaseKind>(base_),
                    kind_name=base_.ToString()
                });
            }

            return response.ToResponse(base_kind_list);

        }

    }
}