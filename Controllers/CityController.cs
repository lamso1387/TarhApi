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
    public class CityController : DefaultController
    {

        public CityController(IDistributedCache distributedCache, ILogger<CityController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن شهر")]
        public async Task<IActionResult> AddCity([FromBody] AddCityRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();

            await Db.AddSave(entity); 

            return response.ToResponse<City>(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date, 
                x.title,
                x.province_id
            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی شهر")]
        public async Task<IActionResult> SearchCity([FromBody] SearchCityRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetCities(request).Paging(response, request.page_start, request.page_size) 
                .Include(x=>x.province)
                 .ToListAsync(); 
            return response.ToResponse<City>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.title, 
                x.province_id,
                x.province_title
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده شهر")]
        public async Task<IActionResult> GetCity(long id)
        {
            SRLCore.Model.SingleResponse<object> response = new SRLCore.Model.SingleResponse<object>();

            var existingEntity = await Db.GetCity(new City { id = id });
            existingEntity.ThrowIfNotExist();
             

            return response.ToResponse< City>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
                x.province_id
            });
        }

        [DisplayName("ویرایش شهر")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditCity([FromBody] AddCityRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetCity(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.title = entity.title;
            existingEntity.province_id = entity.province_id;

            await Db.UpdateSave(); 

            return response.ToResponse< City>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
                x.province_id
            });
        }

        [DisplayName("حذف شهر")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetCity(new City { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}