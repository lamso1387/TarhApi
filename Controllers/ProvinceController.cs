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
    public class ProvinceController : DefaultController
    {

        public ProvinceController(IDistributedCache distributedCache, ILogger<ProvinceController> logger, TarhDb dbContext, UserService userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن استان")]
        public async Task<IActionResult> AddProvince([FromBody] AddProvinceRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();

            await Db.AddSave(entity); 

            return response.ToResponse<Province>(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date, 
                x.title,
            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی استان")]
        public async Task<IActionResult> SearchProvince([FromBody] SearchProvinceRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetProvinces(request).Paging(response, request.page_start, request.page_size)
                
                 .ToListAsync(); 
            return response.ToResponse<Province>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.title, 
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده استان")]
        public async Task<IActionResult> GetProvince(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetProvince(new Province { id = id });
            existingEntity.ThrowIfNotExist();
             

            return response.ToResponse< Province>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
            });
        }

        [DisplayName("ویرایش استان")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditProvince([FromBody] AddProvinceRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetProvince(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.title = entity.title; 

            await Db.UpdateSave(); 

            return response.ToResponse< Province>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
            });
        }

        [DisplayName("حذف استان")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProvince(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetProvince(new Province { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}